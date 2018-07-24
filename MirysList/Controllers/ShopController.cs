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
            IQueryable<CatalogItem> cItems = _dbContext.CataLogItems.Where(x => x.Catalog.Id == catalogId).Include(y => y.Category);
            if (cItems != null)
            {
                return Ok(cItems);
            }
            else
            {
                return NotFound("could not find any catalog items for catalogid " + catalogId);
            }
        }

        // GET: api/Shop/List
        //param : familyId
        [HttpGet]
        [Route("api/Shop/ShoppingList/{familyId}")]
        public IActionResult ShoppingList(int familyId)
        {
            Family familyObj = _dbContext.Families.Where(x => x.Id == familyId).Include(u => u.shoppingList).FirstOrDefault();
            if (familyObj != null)
            {
                ShoppingList listObj = familyObj.shoppingList;
                if (listObj != null)
                {
                    return Ok(listObj);
                }
            }

            return NotFound("could not find a list for this family " + familyId);            
        }

        // GET: api/Shop/ListItems
        //param: listId
        [HttpGet]
        [Route("api/Shop/ShoppingListItems/{shoppinglistId}")]
        public IActionResult ShoppingListItems(int shoppinglistId)
        {
            IQueryable<ShoppingListItem> listItems = _dbContext.ShoppingListItems.Where(x => x.ShoppingList.Id == shoppinglistId).Include(u => u.CatalogItem);
            if (listItems != null)
            {                
                return Ok(listItems);
            }

            return NotFound("could not find a list for this id " + shoppinglistId);            
        }


        // POST: api/Shop/CreateList
        [HttpPost]
        [Route("api/Shop/CreateList/familyId")]        
        public IActionResult CreateList([FromBody]ShoppingList listObj, int familyId)
        {            
            try
            {
                Family familyObj = _dbContext.Families.Where(x => x.Id == familyId).FirstOrDefault();
                if (familyObj != null)
                {
                    _dbContext.ShoppingLists.Add(listObj);
                    foreach (ShoppingListItem item in listObj.listItems)
                    {
                        _dbContext.ShoppingListItems.Add(item);
                    }
                    familyObj.shoppingList = listObj;
                    _dbContext.SaveChanges();
                }
            }
            catch(Exception e)
            {
                return NotFound("could not create a list : " + e.Message);
            }
            
            return Ok(listObj);
        }

        // POST: api/Shop/UpdateList
        [HttpPost]
        [Route("api/Shop/UpdateList/{listId}")]
        public IActionResult UpdateList([FromBody]ShoppingList updatedListObj, int listId)
        {
            ShoppingList listObj;
            try
            {
                listObj = _dbContext.ShoppingLists.Where(x => x.Id == listId).FirstOrDefault();
                if(listObj != null && listObj.Id == updatedListObj.Id)
                {                                      
                    _dbContext.ShoppingLists.Update(updatedListObj);
                    _dbContext.SaveChanges();
                }
            }
            catch(Exception e)
            {
                return NotFound("could not update a list : " + e.Message);
            }

            return Ok(updatedListObj);
        }       
    }
}
