using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.StaticFiles;

namespace MvcMovie.Services;

public record MovieInfo(string? nameRu, string? nameEn, string? nameOriginal, MovieSearchResultFilmCountry[] countries, MovieSearchResultFilmGenre[] genres, 
    double? ratingKinopoisk, double? ratingImdb, int? year, string? type, string? posterUrlPreview, string? posterUrl, double? ratingKinopoiskVoteCount, 
    double? ratingImdbVoteCount, string? description, string? slogan, string? releaseDate, float? filmLength, string? url);
public record MovieSearchResultFilmGenre(string? genre);

public record MovieSearchResultFilmCountry(string? country);

public record MovieSearchResultFilm(int filmId, string? nameRu, string? nameEn, string? nameOriginal, string? type, int? year,
    string? description, string? filmLength, MovieSearchResultFilmCountry[]? countries,
    MovieSearchResultFilmGenre[]? genres, string? rating, int? ratingVoteCount, string? posterUrl, string? posterUrlPreview);

public record MovieSearchResult(string keyword, int pagesCount, int searchFilmsCountResult, MovieSearchResultFilm[] films) : MovieSearchTopResult(pagesCount, films);

public record MovieSearchTopResult(int pagesCount, MovieSearchResultFilm[] films);

public class MovieService
{
    private const string API_KEY = "400b0d53-9d9c-4220-8623-daf95074ffa9";

    private const string API_URL_POPULAR =
        "https://kinopoiskapiunofficial.tech/api/v2.2/films/top?type=TOP_100_POPULAR_FILMS&page=1";

    private const string API_URL_SEARCH =
        "https://kinopoiskapiunofficial.tech/api/v2.1/films/search-by-keyword?keyword=";
    
    private const string API_URL_GET =
        "https://kinopoiskapiunofficial.tech/api/v2.2/films/";

    private static readonly HttpClient client = new()
    {
        DefaultRequestHeaders =
        {
            {
                "X-API-KEY", API_KEY
            }
        }
    };

    public async Task<MovieSearchResult?> GetMovies(string query)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, API_URL_SEARCH + HttpUtility.UrlEncode(query));
        message.Content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
        var request = await client.SendAsync(message);
        return await request.Content.ReadFromJsonAsync<MovieSearchResult>();
    }
    
    public async Task<MovieSearchTopResult?> GetMovies()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, API_URL_POPULAR);
        message.Content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
        var request = await client.SendAsync(message);
        return await request.Content.ReadFromJsonAsync<MovieSearchTopResult>();
    }
    
    public async Task<MovieInfo?> GetMovie(int id)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, API_URL_GET + id);
        message.Content = new StringContent(String.Empty, Encoding.UTF8, "application/json");
        var request = await client.SendAsync(message);
        return await request.Content.ReadFromJsonAsync<MovieInfo>();
    }
}