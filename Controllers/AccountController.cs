using Microsoft.AspNetCore.Mvc;
using QBankApi.Data;
using QBankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace QBankApi.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase 
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> Getaccounts() 
        {
            return await _context.accounts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id) 
        {
            var account = await _context.accounts.FindAsync(id);

            if (account == null) 
            {
                return NotFound();
            }
            
            return account;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account) 
        {
            _context.accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id}, account);
        }
                
        [HttpPut ("{Id}")]
        public async Task<IActionResult> PutAccount (int id, Account account) 
        {
            if (id != account.Id) 
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try 
            {
                await _context.SaveChangesAsync(); 
            } 
            catch (DbUpdateConcurrencyException) 
            {
                if (!_context.accounts.Any(e => e.Id == id)) 
                {
                    return NotFound();
                } 
                else 
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id) 
        {
            
            var account = await _context.accounts.FindAsync(id);

            if (account == null) 
            {
                return NotFound();
            }

            _context.accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}