# Paws & Reservations Documentation

This directory contains supporting design and technical documentation for the Paws & Reservations pet boarding management application.

The root [`README.md`](../README.md) provides the primary project overview, technology stack, major workflows, setup instructions, and build/run information. The documentation in this directory supplements that overview with visual and design artifacts that provide additional context for the application's development.

## Documentation Contents

### Wireframes

The [`wireframes`](wireframes/) directory contains ten selected interface wireframes documenting important application pages and workflows.

See the [wireframe gallery](wireframes/README.md) for an index of the included images and a note about design-stage differences.

### Screenshots

The [application screenshot gallery](screenshots/README.md) contains selected images of the completed application, organized by workflow. It also includes a [sample revenue report PDF](screenshots/report-pdf.pdf). These show the implemented interface; the [wireframes](wireframes/README.md) document its design stage.

### Diagrams

The `diagrams` directory will contain technical and design diagrams that help explain the application's structure and relationships.

Planned diagrams may include:

- Application architecture
- Domain/entity relationships
- Database relationships
- Major business workflows, including linked invoice and payment selections

### Development Database Seeding

The [database seeding guide](../JamesPetBoarding/Scripts/Database/DatabaseSeeding-README.md) documents the repeatable EF6 development baseline, the safe application-data reset, and seed verification. It lives alongside its SQL scripts in the application project.

## Repository Documentation Structure

```text
docs/
├── README.md
├── wireframes/
│   ├── README.md
│   └── 10 selected PNG wireframes
├── screenshots/
│   ├── README.md
│   ├── [application screenshots]
│   └── report-pdf.pdf
└── diagrams/
    └── [technical and design diagrams]
```

## Purpose

These documents are maintained separately from the application source so that supporting design material remains organized without overwhelming the root project README.

Together, the root README and this documentation provide two levels of project information:

1. **Project overview and setup** — the root `README.md`
2. **Supporting design and technical artifacts** — the `docs` directory

## Project Status

The core Paws & Reservations application is complete. The current documentation pass is focused on organizing and presenting the existing project for portfolio and technical review.

Ten selected wireframes are included in `wireframes/`. Application screenshots and a sample revenue report PDF are included in `screenshots/`. Technical diagrams are planned for a later documentation pass.
