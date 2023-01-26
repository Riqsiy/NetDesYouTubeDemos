using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

string pathxml = @"D:\YouTube\NetDes\Tutoriales\GESunat\xml\RRRRRRRRRRR-09-TT01-1.zip";
string filename = Path.GetFileNameWithoutExtension(pathxml);

var bytexml = File.ReadAllBytes(pathxml);
var base64encodestring = Convert.ToBase64String(bytexml);

string hash = "";
using (SHA256 sHA256 = SHA256.Create())
{
    hash = string.Concat(sHA256.ComputeHash(bytexml).Select(x => x.ToString("x2")));
}

string token = "";
string url = $"https://api-cpe.sunat.gob.pe/v1/contribuyente/gem/comprobantes/{filename}";

var odata = new
{
    archivo = new
    {
        nomArchivo = $"{filename}.zip",
        arcGreZip = base64encodestring,
        hashZip = hash
    }
};

var json = JsonConvert.SerializeObject(odata);
using (HttpClient http = new HttpClient())
{
    var body = new StringContent(json,Encoding.UTF8,"application/json");
    http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

    var resp = await http.PostAsync(url, body).ConfigureAwait(false);

    var r = await resp.Content.ReadAsStringAsync();

    Console.WriteLine(r);
}
Console.ReadKey();