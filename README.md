# REST_CSharpServer

Crapy Custom REST client Library in C# .Net Framework, using only Native .Net Framework library and Newtonsoft JSON.net as external library

Not very well tested/documented, so don't use it

Only Works with OAuth2.0 Auth Method (not all OAuth Token method are supported yet)

Use :

Just create and instanciate a REST_Client Object, gives it a Auth Method object if needed and just consume the API using REST_Client.Get<Entity>
  If your request entity is just string, use GetRaw instead
