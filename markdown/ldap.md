# Glossary
Name|Description|Note
---|---|---
LDAP|Lightweight Directory Access Protocol
DIT|Directory Information Tree
DC|Domain Component|Like a folder in file systems
OU|Organization Unit|Like a folder in file systems
CN|Common Name|Like a file in file systems

# LDAP path
Like URLs, the head is smaller and the tail is larger groups
```
LDAP://CN=JohnDoe,OU=SubDepartmentY,OU=DepartmentX,DC=example,DC=com
```

# Active Directory Domain Services providers
* LDAP provider (Since Windows 2000)
* WinNT provider (Since Windows NT 4.0)

# WinNT is outdated
> The WinNT provider on Windows 2000 and later systems has limited functionality

https://msdn.microsoft.com/en-us/library/aa772152.aspx

# References
* [DirectoryEntry.Path](https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices.directoryentry.path)
* [LDAP ADsPath](https://msdn.microsoft.com/en-us/library/aa746384.aspx)
* [WinNT ADsPath](https://msdn.microsoft.com/en-us/library/aa746534.aspx)