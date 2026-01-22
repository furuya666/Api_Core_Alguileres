using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace SERVICES.HttpConectionService
{
    public enum Authentication { None, Header, Basic, Complete }
    public interface IHttpConnectionServices
    {
        Task<T> PostAsync<T>(string _urlBase, string _method, object _request, Authentication _authentication, string _usuario = "", string _password = "");
    }

    public class HttpConnectionServices : IHttpConnectionServices
    {
        public async Task<T> PostAsync<T>(string _urlBase, string _method, object _request, Authentication _authentication, string _usuario = "", string _password = "")
        {
            using (var _httpClient = new HttpClient())
            {
                _httpClient.Timeout = TimeSpan.FromMinutes(5);
                switch (_authentication)
                {
                    case Authentication.None:
                        break;
                    case Authentication.Header:
                        _httpClient.DefaultRequestHeaders.Add("USUARIO", _usuario);
                        _httpClient.DefaultRequestHeaders.Add("PASSWORD", _password);
                        break;
                    case Authentication.Basic:
                        string _encodeBasic = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_usuario + ":" + _password));
                        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {_encodeBasic}");
                        break;
                    case Authentication.Complete:
                        string _encodeComplete = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(_usuario + ":" + _password));
                        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {_encodeComplete}");
                        _httpClient.DefaultRequestHeaders.Add("channel", string.Empty);
                        _httpClient.DefaultRequestHeaders.Add("publicToken", string.Empty);
                        _httpClient.DefaultRequestHeaders.Add("appUserId", string.Empty);
                        break;
                    default:
                        break;
                }
                var _json = JsonConvert.SerializeObject(_request);
                var _content = new StringContent(_json, Encoding.UTF8, "application/json");
                var _result = await _httpClient.PostAsync(new Uri(_urlBase + _method), _content);
                var _resultContent = await _result.Content.ReadAsStringAsync();
                HttpStatusCode _statusCode = _result.StatusCode;
                if (_statusCode == HttpStatusCode.OK)
                {
                    var _response = _resultContent == null ? "" : _resultContent;
                    return JsonConvert.DeserializeObject<T>(_response) ?? throw new InvalidOperationException("Deserialized object is null.");
                }
                else
                {
                    throw new InvalidOperationException($"ERROR => URL METHOD: {_urlBase + _method}; STATUS: {_statusCode}; CONTENT: {_resultContent}");
                }
            }
        }
    }
}
