using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Company.Welcome.Commons;
using Company.Welcome.Entities.GuestVisitor;
using Company.Welcome.Ral.GuestVisitor;

namespace Company.Welcome.Business.GuestVisitor
{
    public class TekGuestVisitorBusinessService : ITekGuestVisitorBusinessService
    {
        private readonly ITekGuestVisitorRepository<VisitorEntity> _tekGuestVisitorRepository;
        private readonly IEncodingHelper _encodingHelper;
        private readonly IProvideDateTime _dateTimeProvider;

        public TekGuestVisitorBusinessService(ITekGuestVisitorRepository<VisitorEntity> tekGuestVisitorRepository,
            IEncodingHelper encodingHelper,
            IProvideDateTime dateTimeProvider)
        {
            if (tekGuestVisitorRepository == null) throw new ArgumentNullException(nameof(tekGuestVisitorRepository));
            if (encodingHelper == null) throw new ArgumentNullException(nameof(encodingHelper));
            if (dateTimeProvider == null) throw new ArgumentNullException(nameof(dateTimeProvider));
            _tekGuestVisitorRepository = tekGuestVisitorRepository;
            _encodingHelper = encodingHelper;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<IEnumerable<Visitor>> GetAllGuest(DateTime date)
        {
            var visitors = new List<Visitor>();
            var entities = _tekGuestVisitorRepository.GetAll();
            foreach (var visitorEntity in entities.ToList())
            {
                var visitor = await CreateVisitor(visitorEntity);
                visitors.Add(visitor);
            }
            var todaysVisitors =
                visitors.Where(
                    v => v.Arrival.Day == date.Day
                    && v.Arrival.Month == date.Month
                    && v.Arrival.Year == date.Year);

            return todaysVisitors;
        }

        public async Task<Visitor> GetGuestDetails(Guid guestId)
        {
            var visitorEntity = _tekGuestVisitorRepository.GetbyId(guestId);
            var visitor = await CreateVisitor(visitorEntity);
            return visitor;
        }

        private async Task<Visitor> CreateVisitor(VisitorEntity visitorEntity)
        {
            var visitor = new Visitor
            {
                Arrival = _dateTimeProvider.FromUnixDateTime(visitorEntity.Arrival),
                Company = visitorEntity.Company,
                Name = visitorEntity.Name,
                Id = visitorEntity.Id,
                Departure = visitorEntity.Departure == 0 
                    ? DateTime.MinValue
                    : _dateTimeProvider.FromUnixDateTime(visitorEntity.Departure) ,
                SignatureImage = await _encodingHelper.FromBase64(visitorEntity.ImageBytesString),
                TekContact = visitorEntity.TekContact,
                Signature = new Signature()
                {
                    ImageBytes = Convert.FromBase64String(visitorEntity.ImageBytesString),
                    PixelWidth = visitorEntity.PixelWidth,
                    PixelHeight = visitorEntity.PixelHeight,
                },
            };
            return visitor;
        }

        public async Task<bool> SaveGuest(Visitor visitor)
        {
            try
            {
                var isMinDate = visitor.Departure.Equals(DateTime.MinValue);
                var timeStamp = !isMinDate
                    ? _dateTimeProvider.ToUnixDateTime(visitor.Departure)
                    : 0;
                var entity = new VisitorEntity
                {
                    Arrival = _dateTimeProvider.ToUnixDateTime(visitor.Arrival),
                    Company = visitor.Company,
                    Name = visitor.Name,
                    Id = visitor.Id,
                    Departure = timeStamp,
                    TekContact = visitor.TekContact,
                    ImageBytesString = await _encodingHelper.ToBase64(visitor.Signature.ImageBytes, (uint)visitor.Signature.PixelHeight, (uint)visitor.Signature.PixelWidth),
                    PixelWidth = visitor.Signature.PixelWidth,
                    PixelHeight = visitor.Signature.PixelHeight
                };
                _tekGuestVisitorRepository.InsertOrUpdate(entity);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return false;
            }
        }

        public Task<bool> GuestIsLeaving(Guid guestId, DateTime departure)
        {
            try
            {
                var visitorEntity = _tekGuestVisitorRepository.GetbyId(guestId);
                visitorEntity.Departure = _dateTimeProvider.ToUnixDateTime(departure);
                _tekGuestVisitorRepository.InsertOrUpdate(visitorEntity);
                return Task.Run(() => true);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return Task.Run(() => false);
            }
        }
    }
}