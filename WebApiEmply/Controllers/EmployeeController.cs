using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiEmply.Controllers
{
    public class EmployeeController : ApiController
    {
        private EmployeeDBEntities _employeeDBEntities;
        public EmployeeController()
        {
            _employeeDBEntities = new EmployeeDBEntities();
        }
        public IEnumerable<Employee> Get()
        {
            return _employeeDBEntities.Employees.ToList();
        }
        public HttpResponseMessage Get(int id)
        {
            Employee employee = _employeeDBEntities.Employees.FirstOrDefault(c => c.Id == id);
            if (employee == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Id'si {id} olan calisan bulunmadi");

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        public HttpResponseMessage Post(Employee employee)
        {
            try
            {
                _employeeDBEntities.Employees.Add(employee);
                if (_employeeDBEntities.SaveChanges() > 0)
                {
                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + employee.Id);
                    return message;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Veri ekleme islemi yapilamadi");
                }
                    
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
