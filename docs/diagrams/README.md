# Technical diagrams

These diagrams summarize the implemented Paws & Reservations application at a high level.

| Diagram | What it shows |
| --- | --- |
| [Application Architecture](application-architecture.png) | Users, the ASP.NET MVC application, identity, data access, LocalDB, email, and PDF generation. |
| [Core Data Relationships](core-data-relationships.png) | The main customer, pet, boarding, invoice, and payment relationships. An invoice's boarding link is optional. |
| [Boarding to Payment Workflow](boarding-payment-workflow.png) | Boarding outcomes, check-in and check-out, invoicing, payment, and the optional payment void path. |

These are overview diagrams, not a complete database schema or a specification of every application path.

## Related documentation

See the [documentation overview](../README.md), [application screenshots](../screenshots/README.md), and [project README](../../README.md).
