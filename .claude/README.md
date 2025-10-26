# Claude Configuration for LongRunningJobsDemo

## Overview
This directory contains Claude-specific configuration files for the LongRunningJobsDemo project.

## Files
- `settings.local.json` - Project-specific permissions and configurations
- `README.md` - This documentation file

## Generated Configuration
- **Generated from**: `/mnt/c/Projects/.claude/settings.global.json`
- **Generated at**: 2025-10-26T16:34:21-04:00
- **Project name**: LongRunningJobsDemo

## Usage
This configuration is automatically applied when Claude starts a session in this project directory.

## Updating Configuration
To regenerate this configuration from the root-level settings:
```bash
/mnt/c/Projects/.claude/init-project.sh LongRunningJobsDemo --force
```

## Root-Level Management
All projects inherit from the global configuration at `/mnt/c/Projects/.claude/settings.global.json`.
To modify permissions for all projects, update the global configuration and re-run the initialization script.
