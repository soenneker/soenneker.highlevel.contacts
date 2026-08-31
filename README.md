[![](https://img.shields.io/nuget/v/soenneker.highlevel.contacts.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.highlevel.contacts/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.highlevel.contacts/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.highlevel.contacts/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.highlevel.contacts.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.highlevel.contacts/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.highlevel.contacts/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.highlevel.contacts/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.HighLevel.Contacts

Upsert, search, retrieve, update, and delete HighLevel contacts through typed request and response models.

## Install

```bash
dotnet add package Soenneker.HighLevel.Contacts
```

## Register

```csharp
using Soenneker.HighLevel.Contacts.Registrars;

services.AddHighLevelContactsUtilAsScoped();
```

The scoped registration keeps the underlying HighLevel client provider singleton, so disposing a scope does not tear down clients reused by later scopes. Use `AddHighLevelContactsUtilAsSingleton()` when the contact service itself should live for the application lifetime.

Set `HighLevel:LogEnabled` to `true` to emit operation-level logs. API keys are supplied per method call, allowing one service instance to work with multiple HighLevel accounts.

## Upsert a contact

```csharp
using Soenneker.HighLevel.Contacts.Abstract;
using Soenneker.HighLevel.OpenApiClient.Models;

var contact = new UpsertContactDto
{
    LocationId = locationId,
    Email = "person@example.com",
    FirstName = "Morgan",
    LastName = "Lee"
};

UpsertContactsSuccessfulResponseDto? response = await contacts.Upsert(
    apiKey,
    contact,
    cancellationToken);
```

`Upsert` and `Update` normalize a non-null email address to lowercase on the supplied model before sending it.

## Look up a contact

```csharp
ContactsByIdSuccessfulResponseDto? byId = await contacts.GetById(
    apiKey,
    contactId,
    cancellationToken);

ContactsSearchSchema? byEmail = await contacts.GetByEmail(
    apiKey,
    "person@example.com",
    locationId,
    cancellationToken);
```

`GetByEmail` performs an advanced search limited to one result and returns the first contact, or `null` when the response contains no match.

## Other operations

```csharp
ContactsSearchSuccessfulResponseDto? matches = await contacts.Search(
    apiKey,
    searchRequest,
    cancellationToken);

UpdateContactsSuccessfulResponseDto? updated = await contacts.Update(
    apiKey,
    contactId,
    updateRequest,
    cancellationToken);

DeleteContactsSuccessfulResponseDto? deleted = await contacts.Delete(
    apiKey,
    contactId,
    cancellationToken);
```

HighLevel error responses are surfaced as exceptions by the generated client; a nullable result means the API returned no usable response body, not that an error was suppressed.
