using AutoMapper;
using NASAData_API.DbModels;
using NASAData_API.Models;
using NASAData_API.Repositories;
using Newtonsoft.Json;

namespace NASAData_API.HostedServices
{
    public class MeteoritesDataWatcherService : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiRepositoryUrl = "";
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IMapper _mapper;
        private readonly IMeteoriteRepository _dbMeteoriteRepository;

        private int _executionCountLimit = 5;
        private Timer? _timer = null;

        public MeteoritesDataWatcherService(IConfiguration configuration, IMapper mapper, IMeteoriteRepository meteoriteRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _dbMeteoriteRepository = meteoriteRepository;
            _apiRepositoryUrl = _configuration.GetSection("SomeApi:NasaRepositoryUrl").Value;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            try
            {
                DbMeteorite[] curDbMeteorites = _dbMeteoriteRepository.GetAll();

                if (curDbMeteorites.Length == 0)
                {
                    _timer = new Timer(DoWork, stoppingToken, TimeSpan.Zero, TimeSpan.FromSeconds(30));
                }
            }
            catch(Exception ex)
            {

            }
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {  
            if(_executionCountLimit == 0) _timer?.Dispose();
            try
            {
                string response = await _httpClient.GetStringAsync(_apiRepositoryUrl);

                Meteorite[] newMeteorites = JsonConvert.DeserializeObject<Meteorite[]>(response);
                DbMeteorite[] newDbMeteorites = _mapper.Map<DbMeteorite[]>(newMeteorites);
                _dbMeteoriteRepository.InsertRange(newDbMeteorites);
                
                _timer?.Dispose();
            }
            catch (Exception ex)
            {
                _executionCountLimit --;
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
           _timer?.Change(Timeout.Infinite, 0); 

            Console.WriteLine("STOP SERVICE");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
