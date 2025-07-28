using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthServer
{
    public static class Config
    {
        //IdentityServer'ın koruyacağı api'ler burada rosource olarak belirtilir. 
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>()
            {
                new ApiResource("resource_api1")
                {
                    Scopes = { "api1.read" , "api1.write" , "api1.update" , "api1.delete" },
                    ApiSecrets = new[] {new Secret("secretapi1".Sha256())}
                    
                },
                new ApiResource("resource_api2")
                {
                    Scopes = { "api2.read", "api2.write", "api2.update" , "api2.delete" },
                     ApiSecrets = new[] {new Secret("secretapi2".Sha256())}
                },
            };
        }
        //Oluşturulan access token'ların api'lerde hangi işlemleri gerçekleştirebilir iziznleri.
        public  static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {

                new ApiScope("api1.read" , "read permission for Api1"),
                new ApiScope("api1.write" , "write permission for Api1"),
                new ApiScope("api1.update" , "write permission for Api1"),
                new ApiScope("api1.delete" , "write permission for Api1"),

                new ApiScope("api2.read" , "read permission for Api2"),
                new ApiScope("api2.write" , "write permission for Api2"),
                new ApiScope("api2.update" , "write permission for Api2"),
                new ApiScope("api2.delete" , "delete permission for Api2")
            };
        }
        //IdentityServer'ın hangi client'lara token göndereceğini belirledik.
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>() {

               new Client()
               {
                   ClientId ="Client1",
                   ClientName ="ClientName1",
                   ClientSecrets = new[] { new Secret("secret1".Sha256())},
                   AllowedGrantTypes = GrantTypes.ClientCredentials, //üyelik içermeyen client'lar için
                   AllowedScopes = {"api1.read" , "api1.update","api2.write" , "api2.update" } //Bu client hangi izinle hangi Api'lere bağlanabilir.

               },

               new Client()
               {
                   ClientId ="Client2",
                   ClientName = "ClientName2",
                   ClientSecrets = new[] { new Secret("secret2".Sha256())},
                   AllowedGrantTypes = GrantTypes.ClientCredentials,
                   AllowedScopes = {"api1.read" , "api2.write" , "api2.update" }
               },


            };
        }


        //Üyelik sisteminde oluşan token'larda user için içerecek claim'ların yapılandırılması.
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>() {
            
                //OpenId token'da user'ın id'sini tutan claim'ı tutar. Burada key'ler identity'den tanımlı keylerdir.
                new IdentityResources.OpenId(),

                //Profile kullanıcı ile ilgili bir çok claim tutar.
                new IdentityResources.Profile()
            };

        }


        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username ="Emre",
                   
                    Password="12345",
                    Claims = new List<Claim>()
                    {
                        new Claim("given_name", "Emre"), //Key profile'da buluanan bir key
                        new Claim("family_name", "Çelik") //key profile'da bulunan bir key 
                    }


                },
                new TestUser
                {
                    SubjectId = "2",
                    Username ="Mehmet",

                    Password="54321",
                    Claims = new List<Claim>()
                    {
                        new Claim("given_name", "Mehmet"), //Key profile'da buluanan bir key
                        new Claim("family_name", "Demir") 
                    }
                },
            };
        }
    }
}

