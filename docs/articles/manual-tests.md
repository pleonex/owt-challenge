# System manual tests

This documents defines the system manual tests for the application.

## Prerequisites

1. [Install .NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. Install `httprepl`: `dotnet tool install -g Microsoft.dotnet-httprepl`
3. Download the version of the software. There are different alternatives.
   - Download the latest binary from the GitHub release
   - Compile the software and run it via the IDE or executable.

## Tests

The tests run via the `httprepl` tool with some scripts:

```bash
httprepl run script.txt
```

Before each test run, re-start the app and remove the database `contacts.db`. If
using docker, just re-start the container.

### Test 1 - CRUD contacts

Script: `test1.txt`

Expected:

1. Entry is created with address in `Bienne`
2. Entry is listed
3. Entry is updated with address in `Lutry`
4. Entry is removed
5. No entries at the end

### Test 2 - CRUD skills

Script: `test2.txt`

Expected

1. Contact without skills added
2. Skill added to contact shows
3. Listing all contacts show no skills
4. Listing all skills show contact
5. Update skill name - new name show
6. Remove skill - no skills show

### Test 3 - Validation

Script: `test3.txt`

Expected

1. Failed to add contact due to missing address
2. Failed to add contact due to invalid email
3. Failed to add contact due to invalid phone
4. Contact added successfully
5. Failed to add skill due to invalid level range
