# Building 'eShop' from Zero to Hero: We create the Web Application with Aspire .NET 9

The starting point for this sample is the following **github repo**:

https://github.com/luiscoco/eShop_Aspire-Tutorial-Step2_CatalogAPI-Configuring_OpenApi

Please donwload the solution in that repo and start implementing the following changes

![image](https://github.com/user-attachments/assets/1c9d510c-827c-4660-a29a-a27666bd8b31)

## 1. We Download and Open the Solution with Visual Studio 2022

![image](https://github.com/user-attachments/assets/eee32d0d-ddfa-474b-99e4-85283ac2030e)

## 2. We create the WebAppComponents project

We right click on the Solution name and we **Add a New Project**

![image](https://github.com/user-attachments/assets/9e8770b5-61c3-4bac-95bd-8ffd1c154a2f)

We select the **Blazor Web App** project template and press the Next button 

![image](https://github.com/user-attachments/assets/df365ec2-fb1d-4c24-8079-2ff777b830ef)

We set the **Project Name and Location** and press the Next button 

![image](https://github.com/user-attachments/assets/77517150-8091-4e6d-ba8a-957600fabdab)

We select the **.NET 9** Framework and press the Create button

![image](https://github.com/user-attachments/assets/48183b0e-6eca-4e3a-812c-763e9716eb13)

We verify the new project added in the solution

![image](https://github.com/user-attachments/assets/6d334da8-87ff-4d40-a2e1-4ac895915ac1)

## 3. We modify the WebAppComponents project

We delete/remove from the Blazor Web App project these folders and files:

![image](https://github.com/user-attachments/assets/2a37fbc2-738b-44de-af0c-722d39404ef8)

This is the project structure

![image](https://github.com/user-attachments/assets/47413af9-1b8e-403f-add8-db24aceed90d)

## 4. We load the Nuget Package in the WebAppComponents project

**Microsoft.AspNetCore.Components.Web**

![image](https://github.com/user-attachments/assets/cc0352d3-806a-45cc-a063-30593d35353b)

## 5. We edit the WebAppComponents.csproj file

We replace the actual **Project Sdk "Microsoft.NET.Sdk.Web"**

![image](https://github.com/user-attachments/assets/39dcb3b6-bbe9-4133-b6cb-3528a5627918)

We set a new **Project Sdk "Microsoft.NET.Sdk.Razor"**

![image](https://github.com/user-attachments/assets/befeda3b-fe47-4c28-930d-90d6aa1f420f)

## 6. We create new folders in the WebAppComponents project 

We create these folder in the project

![image](https://github.com/user-attachments/assets/08e490d2-7748-486d-beec-d80d729bb5ed)

## 7. We add the Sevices in the WebAppComponents project 

**ICatalogService.cs**

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services
{
    public interface ICatalogService
    {
        Task<CatalogItem?> GetCatalogItem(int id);
        Task<CatalogResult> GetCatalogItems(int pageIndex, int pageSize, int? brand, int? type);
        Task<List<CatalogItem>> GetCatalogItems(IEnumerable<int> ids);
        Task<CatalogResult> GetCatalogItemsWithSemanticRelevance(int page, int take, string text);
        Task<IEnumerable<CatalogBrand>> GetBrands();
        Task<IEnumerable<CatalogItemType>> GetTypes();
    }
}
```

**IProductImageUrlProvider.cs**

```csharp
using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services;

public interface IProductImageUrlProvider
{
    string GetProductImageUrl(CatalogItem item)
        => GetProductImageUrl(item.Id);

    string GetProductImageUrl(int productId);
}
```

**CatalogService.cs**

```csharp
using System.Net.Http.Json;
using System.Web;
using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Services;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    private readonly string remoteServiceBaseUrl = "/api/catalog/";

    public Task<CatalogItem?> GetCatalogItem(int id)
    {
        var uri = $"{remoteServiceBaseUrl}items/{id}";
        return httpClient.GetFromJsonAsync<CatalogItem>(uri);
    }

    public async Task<CatalogResult> GetCatalogItems(int pageIndex, int pageSize, int? brand, int? type)
    {
        var uri = GetAllCatalogItemsUri(remoteServiceBaseUrl, pageIndex, pageSize, brand, type);
        var result = await httpClient.GetFromJsonAsync<CatalogResult>(uri);
        return result!;
    }

    public async Task<List<CatalogItem>> GetCatalogItems(IEnumerable<int> ids)
    {
        var uri = $"{remoteServiceBaseUrl}items/by?ids={string.Join("&ids=", ids)}";
        var result = await httpClient.GetFromJsonAsync<List<CatalogItem>>(uri);
        return result!;
    }

    public Task<CatalogResult> GetCatalogItemsWithSemanticRelevance(int page, int take, string text)
    {
        var url = $"{remoteServiceBaseUrl}items/withsemanticrelevance/{HttpUtility.UrlEncode(text)}?pageIndex={page}&pageSize={take}";
        var result = httpClient.GetFromJsonAsync<CatalogResult>(url);
        return result!;
    }

    public async Task<IEnumerable<CatalogBrand>> GetBrands()
    {
        var uri = $"{remoteServiceBaseUrl}catalogBrands";
        var result = await httpClient.GetFromJsonAsync<CatalogBrand[]>(uri);
        return result!;
    }

    public async Task<IEnumerable<CatalogItemType>> GetTypes()
    {
        var uri = $"{remoteServiceBaseUrl}catalogTypes";
        var result = await httpClient.GetFromJsonAsync<CatalogItemType[]>(uri);
        return result!;
    }

    private static string GetAllCatalogItemsUri(string baseUri, int pageIndex, int pageSize, int? brand, int? type)
    {
        string filterQs;

        if (type.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/{type.Value}/brand/{brandQs}";
        }
        else if (brand.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/all/brand/{brandQs}";
        }
        else
        {
            filterQs = string.Empty;
        }
        return $"{baseUri}items{filterQs}?pageIndex={pageIndex}&pageSize={pageSize}";
    }
}
```


## 8.



## 9.



## 10.




