(function () {
    "use strict";

    var collator = new Intl.Collator(undefined, { numeric: true, sensitivity: "base" });
    var emptyValues = /^(?:|n\/a|none|no (?:pet|customer|boarding|diet|medication)|—|–|-)$/i;

    function valueFor(cell, type) {
        var raw = cell.getAttribute("data-sort-value");
        var value = (raw === null ? cell.textContent : raw).replace(/\s+/g, " ").trim();
        if (emptyValues.test(value)) return null;

        if (type === "timing" && /expires today/i.test(value)) return 0;
        if (type === "number" || type === "duration" || type === "timing") {
            var match = value.replace(/,/g, "").match(/-?\d+(?:\.\d+)?/);
            if (!match) return null;
            var number = Number(match[0]);
            if (type === "duration") {
                var days = value.match(/(\d+(?:\.\d+)?)\s*day/i);
                var hours = value.match(/(\d+(?:\.\d+)?)\s*hour/i);
                var minutes = value.match(/(\d+(?:\.\d+)?)\s*min/i);
                return (days ? Number(days[1]) * 1440 : 0) +
                    (hours ? Number(hours[1]) * 60 : 0) +
                    (minutes ? Number(minutes[1]) : 0) || number;
            }
            if (type === "timing" && /since expiration/i.test(value)) return -number;
            if (type === "timing" && /expires today/i.test(value)) return 0;
            if (/^\s*\(/.test(value)) return -number;
            return number;
        }

        if (type === "date") {
            var date = value.match(/^(\d{1,2})\/(\d{1,2})\/(\d{4})(?:,?\s+(\d{1,2}):(\d{2})(?::\d{2})?\s*(AM|PM)?)?/i);
            if (!date) return null;
            var hour = Number(date[4] || 0);
            if (date[6]) hour = hour % 12 + (date[6].toUpperCase() === "PM" ? 12 : 0);
            var stamp = new Date(Number(date[3]), Number(date[1]) - 1, Number(date[2]), hour, Number(date[5] || 0));
            if (stamp.getFullYear() !== Number(date[3]) || stamp.getMonth() !== Number(date[1]) - 1 || stamp.getDate() !== Number(date[2])) return null;
            return stamp.getTime();
        }
        return value;
    }

    function initialize(table) {
        var body = table.tBodies[0];
        if (!body) return;
        var headings = Array.prototype.slice.call(table.querySelectorAll("thead th[data-sort-type]"));
        headings.forEach(function (heading) {
            var button = heading.querySelector("button");
            if (!button) return;
            button.addEventListener("click", function () {
                var ascending = heading.getAttribute("aria-sort") !== "ascending";
                var direction = ascending ? 1 : -1;
                var column = Array.prototype.indexOf.call(heading.parentNode.children, heading);
                var type = heading.getAttribute("data-sort-type");
                var rows = Array.prototype.map.call(body.rows, function (row, index) {
                    return { row: row, index: index, value: valueFor(row.cells[column], type) };
                });
                rows.sort(function (left, right) {
                    if (left.value === null) return right.value === null ? left.index - right.index : 1;
                    if (right.value === null) return -1;
                    var result = type === "text" ? collator.compare(left.value, right.value) : left.value - right.value;
                    return result ? result * direction : left.index - right.index;
                });
                rows.forEach(function (item) { body.appendChild(item.row); });
                headings.forEach(function (other) {
                    other.setAttribute("aria-sort", other === heading ? (ascending ? "ascending" : "descending") : "none");
                    other.querySelector("button").querySelector(".report-sort-indicator").textContent =
                        other === heading ? (ascending ? " ▲" : " ▼") : " ↕";
                });
            });
        });
    }

    document.querySelectorAll("table.report-sortable").forEach(initialize);
}());