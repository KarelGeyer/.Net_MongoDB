using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Services;

public class MongoDBService
{
    private readonly IMongoCollection<Playlist> _playlistCollection;
    private readonly IMongoCollection<MovieList> _movielistCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _playlistCollection = database.GetCollection<Playlist>(mongoDBSettings.Value.CollectionName[0]);
        _movielistCollection = database.GetCollection<MovieList>(mongoDBSettings.Value.CollectionName[1]);
    }

    // Playlist Collection
    public async Task<List<Playlist>> GetAsync() 
    {
        return await _playlistCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task CreateAsync(Playlist playlist)
    {
        await _playlistCollection.InsertOneAsync(playlist);
        return;
    }

    public async Task AddToPlaylistAsync(string id, string movieId)
    {
        FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
        UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("items", movieId);

        await _playlistCollection.UpdateOneAsync(filter, update);
        return;
    }

    public async Task DeleteAsync(string id)
    {
        FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);

        await _playlistCollection.DeleteOneAsync(filter);
        return;
    }

    // MovieList Collection
    public async Task<List<MovieList>> GetMoviesAsync()
    {
        var movieList = await _movielistCollection.Find(new BsonDocument()).ToListAsync();
        return movieList;
    }

    public async Task CreateMovieAsync(MovieList movieList)
    {
        Console.WriteLine(movieList.Id);
        await _movielistCollection.InsertOneAsync(movieList);
        return;
    }

    public async Task DeleteMovieAsync(string id)
    {
        FilterDefinition<MovieList> thisId = Builders<MovieList>.Filter.Eq("Id", id);
        await _movielistCollection.DeleteOneAsync(thisId); 
        return;
    }
}
