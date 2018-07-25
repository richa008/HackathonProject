using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MirysList.Models;
using Newtonsoft.Json;

namespace MirysList.Controllers
{
    [Produces("application/json")]    
    public class ShopController : Controller
    {
        private AppDbContext _dbContext;

        public ShopController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Shop/Catalog
        [HttpGet]
        [Route("api/Shop/Catalog")]
        public IActionResult Catalog()
        {
            return Ok(_dbContext.Catalogs);
        }

        // GET: api/Shop/CatalogItems
        //param: catalogId
        [HttpGet]
        [Route("api/Shop/Catalog/{catalogId}")]
        public IActionResult CataLogItems(int catalogId)
        {


            Catalog catalog = _dbContext.Catalogs.Where(x => x.Id == catalogId).Include(c => c.Items).FirstOrDefault();
            if (catalog != null)
            {
                ICollection<CatalogItem> cItems = catalog.Items;
                if (cItems != null)
                {
                    return Ok(cItems);
                }
            }

            return NotFound("could not find any catalog items for catalogid " + catalogId);            
        }

        // GET: api/Shop/List
        //param : familyId
        [HttpGet]
        [Route("api/Shop/ShoppingList/{familyId}")]
        public IActionResult ShoppingList(int familyId)
        {
            Family familyObj = _dbContext.Families.Where(x => x.Id == familyId).Include(u => u.listItems).FirstOrDefault();
            if (familyObj != null)
            {
                List<ShoppingListItem> listItems = familyObj.listItems;
                List<ShoppingListItem> resultItems = new List<ShoppingListItem>();
                if (listItems != null)
                {
                    foreach(ShoppingListItem item in listItems)
                    {
                        ShoppingListItem it = _dbContext.ShoppingListItems.Where(x => x.Id == item.Id).FirstOrDefault();
                        resultItems.Add(it);
                    }

                    return Ok(resultItems);
                }
            }

            return NotFound("could not find a list for this family " + familyId);            
        }

        // GET: api/Shop/ListItems
        //param: listId
        /*[HttpGet]
        [Route("api/Shop/ShoppingListItems/{shoppinglistId}")]
        public IActionResult ShoppingListItems(int shoppinglistId)
        {
            ShoppingList list = _dbContext.ShoppingLists.Where(x => x.Id == shoppinglistId).FirstOrDefault();
            if (list != null)
            {
                ICollection<ShoppingListItem> listItems = list.listItems;
                if (listItems != null)
                {
                    return Ok(listItems);
                }
            }

            return NotFound("could not find a list for this id " + shoppinglistId);            
        }*/


        // POST: api/Shop/CreateList
        [HttpPost]
        [Route("api/Shop/CreateList/{familyId}")]        
        public IActionResult CreateList([FromBody]List<ShoppingListItem> listItems, int familyId)
        {
            List<UpdatedShoppingListItem> result = new List<UpdatedShoppingListItem>();
            Family familyObj = null;
            try
            {
                familyObj = _dbContext.Families.Where(x => x.Id == familyId).Include(y => y.listItems).FirstOrDefault();
                if (familyObj != null)
                {                   
                    foreach (ShoppingListItem item in listItems)
                    {
                        CatalogItem i = _dbContext.CataLogItems.Where(x => x.Id == item.CatalogItemId).FirstOrDefault();
                        if (i != null)
                        {
                            _dbContext.ShoppingListItems.Add(item);
                            UpdatedShoppingListItem resultItem = new UpdatedShoppingListItem(item);                            
                            resultItem.CatalogItem = i;
                            familyObj.listItems.Add(item);
                            result.Add(resultItem);
                        }
                    }

                    _dbContext.Families.Update(familyObj);
                    _dbContext.SaveChanges();
                    return Ok(result);
                }
            }
            catch(Exception e)
            {
                return NotFound("could not create a list : " + e);
            }
            
            return NotFound("could not create a list");
        }

        // POST: api/Shop/UpdateList
        [HttpPost]
        [Route("api/Shop/UpdateList/{familyId}")]
        public IActionResult UpdateList([FromBody]List<ShoppingListItem> updatedListItems, int familyId)
        {
            Family familyObj;
            try
            {
                familyObj = _dbContext.Families.Where(x => x.Id == familyId).FirstOrDefault();
                if (familyObj != null)
                {
                    foreach (ShoppingListItem item in updatedListItems)
                    {
                        _dbContext.ShoppingListItems.Update(item);
                        _dbContext.SaveChanges();
                    }
                }                
            }
            catch(Exception e)
            {
                return NotFound("could not update a list : " + e.Message);
            }

            return Ok(updatedListItems);
        }       
    }
}
