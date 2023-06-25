# Items API - Nuke Demo

## Installation

* Install the `nuke` global tool

```shell
dotnet tool install Nuke.Global --global
```

* Create a solution, and at the root level, run the below command

```shell
shell :setup
```

Then follow the setup steps to get a `nuke` build to your solution.

## Build Pipeline

```mermaid
flowchart LR
    Clean --> Restore
    Restore --> Build
    Build --> Test
```

## Todo

- [ ] Include CSharpier Format Checks
