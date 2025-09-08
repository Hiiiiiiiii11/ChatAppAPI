using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public interface IUploadPhotoService
    {
        string UploadPhotoAsync(IFormFile file);
    }
}
