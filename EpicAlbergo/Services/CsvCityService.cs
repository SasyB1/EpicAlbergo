using EpicAlbergo.Models.Dto;

namespace EpicAlbergo.Services
{
    public class CsvCityService
    {
        private readonly List<CityDto> _cityList = [];

        public CsvCityService(IWebHostEnvironment env)
        {
            var filename = Path.Combine(env.WebRootPath, @"CsvData\comuniitaliani.csv");
            File.ReadAllLines(filename, System.Text.Encoding.Default) // leggo tutte le righe del file
                .Skip(1) // scarto la prima riga di intestazione
                .Select(line => line.Split(';')) // suddivido le righe per ottenere i campi di ognuna
                                                 // fields è un array di stringhe
                .Select(fields => new CityDto
                { // ottengo le città corrispondenti alle righe del file 
                    Id = int.Parse(fields[4]),
                    CadastralCode = fields[19],
                    Name = fields[5],
                    Province = new ProvinceDto
                    {
                        Id = int.Parse(fields[2]),
                        Name = fields[11],
                        Acronym = fields[14],
                    }
                }).ToList() // trasformo in lista
                .ForEach(c => _cityList.Add(c)); // agggiungo tutte le città alla lista delle città
        }

        public IEnumerable<CityDto> GetByProvince(string acronym) =>
            // where fa passare solo gli elementi che soddisfano il predicato (lambda) passato come parametro
            _cityList.Where(c => c.Province.Acronym == acronym);

        public CityDto GetCityById(int id) => _cityList.Single(c => c.Id == id);

        public CityDto GetCityByName(string cityName) =>
            // first recupera la prima occorrenza che soddisfa il predicato
            _cityList.First(c => c.Name.Equals(cityName, StringComparison.InvariantCultureIgnoreCase));

        public IEnumerable<ProvinceDto> GetProvinces() =>
            // select trasforma la lista originaria secondo la funzione parametro
            _cityList.Select(c => c.Province)
            // distinct elimina i duplicati
            .Distinct();

        
    }
}
