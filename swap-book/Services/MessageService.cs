using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using swap_book.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;


namespace swap_book.Services
{
	public interface IMessageService
	{
		Task SendMessage(ApplicationUser sender, ApplicationUser recipient, string content);
		Task SendMessage(ApplicationUser sender, ApplicationUser recipient, Message message);
		Task<List<Message>> GetMessages(ApplicationUser user);
		Task<List<Message>> GetUnreadMessages(ApplicationUser user);
		Task MarkMessageAsRead(Message message);
        Task<List<Message>> GetСonfirmMessages(ApplicationUser user);
        Task<int> CountConfiimMessages(ApplicationUser user);
    }
	public class MessageService : IMessageService
	{

		private readonly DatabaseContext _context;


		public MessageService(DatabaseContext context)
		{
			_context = context;
		}

		public async Task SendMessage(ApplicationUser sender, ApplicationUser recipient, string content)
		{
			if (sender == null || recipient == null)
			{
				throw new ArgumentNullException();
			}

			if (string.IsNullOrEmpty(content))
			{
				throw new ArgumentException("Message content cannot be empty.");
			}

			var message = new Message
			{
				SenderId = sender.Id,
				RecipientId = recipient.Id,
				Content = content,
				SentDate = DateTime.UtcNow,
				IsRead = false
			};

			await _context.Messages.AddAsync(message);
			await _context.SaveChangesAsync();
		}
		public async Task SendMessage(ApplicationUser sender, ApplicationUser recipient, Message message)
		{
			if (sender == null || recipient == null)
			{
				throw new ArgumentNullException();
			}

			if (string.IsNullOrEmpty(message.Content))
			{
				throw new ArgumentException("Message content cannot be empty.");
			}

			message.SenderId = sender.Id;
			message.RecipientId = recipient.Id;
			message.SentDate = DateTime.UtcNow;
            message.IsRead = false;

			await _context.Messages.AddAsync(message);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Message>> GetMessages(ApplicationUser user)
		{
			return await _context.Messages
				.Where(m => m.SenderId == user.Id || m.RecipientId == user.Id)
				.ToListAsync();
		}
        public async Task<List<Message>> GetСonfirmMessages(ApplicationUser user)
        {
            return await _context.Messages
                .Where(m => m.SenderId == user.Id || m.RecipientId == user.Id)
                .Where(m => m.Content.StartsWith("You have received a book exchange request from"))  // Filter by content
                .ToListAsync();
        }

        public async Task<int> CountConfiimMessages(ApplicationUser user)
        {
            return await _context.Messages
                .Where(m => m.SenderId == user.Id || m.RecipientId == user.Id)
                .Where(m => m.Content.StartsWith("You have received a book exchange request from"))
                .CountAsync();
        }
        public async Task<List<Message>> GetUnreadMessages(ApplicationUser user)
		{
			return await _context.Messages
				.Where(m => m.RecipientId == user.Id && !m.IsRead)
				.ToListAsync();
		}

		public async Task MarkMessageAsRead(Message message)
		{
			if (message == null)
			{
				throw new ArgumentNullException();
			}

			message.IsRead = true;
			_context.Update(message);
			await _context.SaveChangesAsync();
		}
	}
}
