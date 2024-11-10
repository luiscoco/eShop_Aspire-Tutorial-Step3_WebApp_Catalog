# Building 'eShop' from Zero to Hero: We create the Web Application with Aspire .NET 9

The starting point for this sample is the following **github repo**:

https://github.com/luiscoco/eShop_Aspire-Tutorial-Step2_CatalogAPI-Configuring_OpenApi

Please donwload the solution in that repo and start implementing the following changes

![image](https://github.com/user-attachments/assets/1c9d510c-827c-4660-a29a-a27666bd8b31)

## 1. We Download and Open the Solution with Visual Studio 2022

![image](https://github.com/user-attachments/assets/eee32d0d-ddfa-474b-99e4-85283ac2030e)

## 2. We Create the WebAppComponents Project

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

## 3. We Modify the WebAppComponents Project

We delete/remove from the Blazor Web App project these folders and files:

![image](https://github.com/user-attachments/assets/2a37fbc2-738b-44de-af0c-722d39404ef8)

This is the project structure

![image](https://github.com/user-attachments/assets/47413af9-1b8e-403f-add8-db24aceed90d)

## 4. We Load the Nuget Package in the WebAppComponents Project

**Microsoft.AspNetCore.Components.Web**

![image](https://github.com/user-attachments/assets/cc0352d3-806a-45cc-a063-30593d35353b)

## 5. We Edit the WebAppComponents.csproj File

We replace the actual **Project Sdk "Microsoft.NET.Sdk.Web"**

![image](https://github.com/user-attachments/assets/39dcb3b6-bbe9-4133-b6cb-3528a5627918)

We set a new **Project Sdk "Microsoft.NET.Sdk.Razor"**

![image](https://github.com/user-attachments/assets/befeda3b-fe47-4c28-930d-90d6aa1f420f)

## 6. We Create new folders in the WebAppComponents Project 

We create these folder in the project

![image](https://github.com/user-attachments/assets/08e490d2-7748-486d-beec-d80d729bb5ed)

## 7. We Add the Sevices in the WebAppComponents Project 

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


## 8. We Add the ItemHelper.cs File in the WebAppComponents Project 


```csharp
using eShop.WebAppComponents.Catalog;

namespace eShop.WebAppComponents.Item;

public static class ItemHelper
{
    public static string Url(CatalogItem item)
        => $"item/{item.Id}";
}
```

## 9. We add the Razor Componentes in the WebAppComponents Project 

**CatalogItem.cs**

```csharp
namespace eShop.WebAppComponents.Catalog;

public record CatalogItem(
    int Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    int CatalogBrandId,
    CatalogBrand CatalogBrand,
    int CatalogTypeId,
    CatalogItemType CatalogType);

public record CatalogResult(int PageIndex, int PageSize, int Count, List<CatalogItem> Data);
public record CatalogBrand(int Id, string Brand);
public record CatalogItemType(int Id, string Type);
```

**CatalogListItem.razor**

```razor
@using eShop.WebAppComponents.Item
@inject IProductImageUrlProvider ProductImages

<div class="catalog-item">
    <a class="catalog-product" href="@ItemHelper.Url(Item)" data-enhance-nav="false">
        <span class='catalog-product-image'>
            <img alt="@Item.Name" src='@ProductImages.GetProductImageUrl(Item)' />
        </span>
        <span class='catalog-product-content'>
            <span class='name'>@Item.Name</span>
            <span class='price'>$@Item.Price.ToString("0.00")</span>
        </span>
    </a>
</div>

@code {
    [Parameter, EditorRequired]
    public required CatalogItem Item { get; set; }

    [Parameter]
    public bool IsLoggedIn { get; set; }
}
```

**CatalogSearch.razor**

```razor
@inject CatalogService CatalogService
@inject NavigationManager Nav

@if (catalogBrands is not null && catalogItemTypes is not null)
{
    <div class="catalog-search">
        <div class="catalog-search-header">
            <img role="presentation" src="icons/filters.svg" />
            Filters
        </div>
        <div class="catalog-search-types">
            <div class="catalog-search-group">
                <h3>Brand</h3>
                <div class="catalog-search-group-tags">
                    <a href="@BrandUri(null)"
                    class="catalog-search-tag @(BrandId == null ? "active " : "")">
                        All
                    </a>
                    @foreach (var brand in catalogBrands)
                    {
                        <a href="@BrandUri(brand.Id)"
                        class="catalog-search-tag @(BrandId == brand.Id ? "active " : "")">
                            @brand.Brand
                        </a>
                    }
                </div>
            </div>
            <div class="catalog-search-group">
                <h3>Type</h3>

                <div class="catalog-search-group-tags">
                    <a href="@TypeUri(null)"
                    class="catalog-search-tag @(ItemTypeId == null ? "active " : "")">
                    All
                    </a>
                    @foreach (var itemType in catalogItemTypes)
                    {
                        <a href="@TypeUri(itemType.Id)"
                        class="catalog-search-tag @(ItemTypeId == itemType.Id ? "active " : "")">
                            @itemType.Type
                        </a>
                    }
                </div>
            </div>
        </div>
    </div>
}

@code {
    IEnumerable<CatalogBrand>? catalogBrands;
    IEnumerable<CatalogItemType>? catalogItemTypes;
    [Parameter] public int? BrandId { get; set; }
    [Parameter] public int? ItemTypeId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var brandsTask = CatalogService.GetBrands();
        var itemTypesTask = CatalogService.GetTypes();
        await Task.WhenAll(brandsTask, itemTypesTask);
        catalogBrands = brandsTask.Result;
        catalogItemTypes = itemTypesTask.Result;
    }

    private string BrandUri(int? brandId) => Nav.GetUriWithQueryParameters(new Dictionary<string, object?>()
    {
        { "page", null },
        { "brand", brandId },
    });

    private string TypeUri(int? typeId) => Nav.GetUriWithQueryParameters(new Dictionary<string, object?>()
    {
        { "page", null },
        { "type", typeId },
    });
}
```

## 10. We Create a Blazor Web Application

We right click on the Solution and we **Add a New Project...**

![image](https://github.com/user-attachments/assets/cb9c616b-a0c4-4b15-9217-257a52ad447e)

We select the **Blazor Web Application** as project template and press the Next button

![image](https://github.com/user-attachments/assets/17bc048b-8e01-4de7-b823-144c14a5de6d)

We input the project name and location and press the Next button

![image](https://github.com/user-attachments/assets/657c3b3e-1d9b-411c-9102-72784a820052)

We select the **.NET 9 Framework** and we press the Create button

![image](https://github.com/user-attachments/assets/21daa4f7-c5a2-4edb-b532-952fc4ee2405)

We review the Solution folders structure

![image](https://github.com/user-attachments/assets/a0956d4c-6daf-489e-95b9-104d52c1075b)

## 11. We add the Project References in the Blazor Web Application

![image](https://github.com/user-attachments/assets/7644991d-db01-4353-bca9-16db01f4a5fc)

![image](https://github.com/user-attachments/assets/90cffc03-cff0-46a0-9542-2da9e651ff5d)

We verify the two project references were added

![image](https://github.com/user-attachments/assets/2da2482d-cf61-44b4-8399-24fa5b263539)

## 12. We add the Nuget Packages in the Blazor Web Application

We add the following packages in the WebApp project

**Asp.Versioning.Http.Client**: This library is part of ASP.NET API Versioning tools, designed to help developers handle versioning for their HTTP API clients. It allows you to specify and manage different versions of HTTP APIs in a way that’s seamless and compatible with ASP.NET's API versioning framework. Using this library, clients can easily specify the API version they want to call, making it useful for applications that need to interact with multiple versions of an API or ensure compatibility with evolving APIs.

**Microsoft.Extensions.ServiceDiscovery.Yarp**: This package is a service discovery extension for YARP (Yet Another Reverse Proxy) in the Microsoft.Extensions ecosystem. It enables dynamic service discovery for applications using YARP, allowing YARP to automatically locate and route requests to the appropriate service endpoints. This is especially useful in microservices architectures, where services might scale dynamically or move between different hosts. It typically integrates with service registries like Consul, Eureka, or Azure Service Fabric.

**Yarp.ReverseProxy**: YARP (Yet Another Reverse Proxy) is an open-source reverse proxy library created by Microsoft, built on .NET. Yarp.ReverseProxy is the core package, providing essential features for creating a reverse proxy server, including request routing, load balancing, and protocol support (such as HTTP/1.1 and HTTP/2). It’s highly customizable and can be used to route client requests to various backend services, making it valuable in distributed systems and microservices setups.

## 13. We Add the Services Folder

![image](https://github.com/user-attachments/assets/36f4915f-e408-4d76-949b-ff0658ca2a52)

![image](https://github.com/user-attachments/assets/194c9c34-b158-42ab-8887-a445fc93e1bb)

## 14. We Add the 

![image](https://github.com/user-attachments/assets/7403827c-828a-459a-bc4e-8a53e104648e)

**ProductImageUrlProvider.cs**

```csharp
using eShop.WebAppComponents.Services;

namespace eShop.WebApp.Services;

public class ProductImageUrlProvider : IProductImageUrlProvider
{
    public string GetProductImageUrl(int productId)
        => $"product-images/{productId}?api-version=1.0";
}
```






