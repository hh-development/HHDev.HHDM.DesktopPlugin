# HH Data Management Example Plugin

This repository contains a sample project that can be used to get started with developing a plugin for [HH Data Management](https://hh-dev.com/HHDataManagement).

## Why are plugins needed

The plugin framework in HH Data Management allows users to implement their own custom logic in the software. There are numerous customization features built into HH Data Management, but sometimes these are not sufficient to represent a required workflow or functionality.  In these cases, a plugin can be written in C# which greatly increases the flexibility for implementing custom functionality.  The plugin framework provides various simple access points that make it possible to quickly add minor customizations or alternatively allow custom displays to be created from scratch.  In theory, all of the default displays used in HH Data Management could be fully recreated using the plugin framework.

### Examples

Some examples of the customizations possible in the plugin framework are:
* create custom calculated properties on any items (setup, run, lap etc.)
* add custom columns to any of the tables that are linked to calculated values, or values stored in the database
* create a custom run plan calculator
* customize the built-in tyre pressure calculations
* create custom user interfaces for the run, or event-specific event and session views
* create custom views, that make it possible to create:
* custom dashboards
* custom import/export functionality (for example to integrate with other tools with requirements that cannot be met using the default export functionality already built into the software)
* entirely custom entities in the software by using the custom definitions

# Using the example plugin

This example project contains the requisite files and project structure to get started developing any plugin for HH Data Management. However, some changes unique to your development environment and HH Data Management account are required for the plugin to be built. Please see [the help file](https://help.hh-dm.com/extensibility/plugins/getting-started-with-example-plugin) for instructions on how to build and run the example plugin.
