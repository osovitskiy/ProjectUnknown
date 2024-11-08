# ProjectUnknown.AspNetCore.DataProtection.MongoDB

MongoDB implementation of IXmlRepository.

## Usage
```
services.AddDataProtection()
    .PersistKeysToMongo("mongodb://localhost", "MyDatabase", "MyCollection");
```
