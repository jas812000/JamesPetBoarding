# Paws & Reservations Documentation

This directory contains supporting design and technical documentation for the Paws & Reservations pet boarding management application.

The root [`README.md`](../README.md) provides the primary project overview, technology stack, major workflows, setup instructions, and build/run information. The documentation in this directory supplements that overview with visual and design artifacts that provide additional context for the application's development.

## Documentation Contents

### Wireframes

The [`wireframes`](wireframes/) directory is reserved for interface wireframes documenting important application pages and workflows.

The wireframes are pending and will be added as part of the project's documentation and portfolio presentation work. See the [`wireframes/README.md`](wireframes/README.md) file for the current wireframe plan and status.

### Screenshots

The `screenshots` directory will contain selected screenshots of the completed application.

Screenshots are intended to demonstrate important implemented workflows and user interfaces without requiring a reviewer to run the application locally.

Planned screenshot coverage includes representative areas such as:

- Public-facing pages
- Staff and management dashboards
- Customer and pet management
- Boarding workflows
- Invoicing and payment processing
- Reporting and PDF export
- Employee profile photos and the public Our Team page
- Email security-code verification and account settings

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
│   └── [wireframe files]
├── screenshots/
│   └── [application screenshots]
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

Screenshots, wireframes, and diagrams are planned but are not yet present. Add each artifact to its directory, update the relevant index here or in `wireframes/README.md`, and link a small representative selection from the root README.
