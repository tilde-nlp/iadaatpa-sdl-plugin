using Newtonsoft.Json;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestClient
{
    class MyTranslationProviderCredentialStore : ITranslationProviderCredentialStore
    {
        public Dictionary<string, string> Credentials { get; set; } = new Dictionary<string, string>();

        public void AddCredential(Uri uri, TranslationProviderCredential credential)
        {
            Credentials[uri.ToString()] = credential.Credential;
        }

        public void Clear() => Credentials.Clear();

        public TranslationProviderCredential GetCredential(Uri uri)
        {
            Credentials.TryGetValue(uri.ToString(), out string val);
            return val == null ? null : new TranslationProviderCredential(val, true); // TODO: use the real persist value
        }
        public void RemoveCredential(Uri uri) => Credentials.Remove(uri.ToString());

        public static void ToFile(string path, MyTranslationProviderCredentialStore obj)
        {
            string serialized = JsonConvert.SerializeObject(obj);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(serialized);
            }
        }

        public static MyTranslationProviderCredentialStore FromFile(string path)
        {
            string serialized;
            using(StreamReader reader = new StreamReader(path))
            {
                serialized = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<MyTranslationProviderCredentialStore>(serialized);
        }
    }
}
