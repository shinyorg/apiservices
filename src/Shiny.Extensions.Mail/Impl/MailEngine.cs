using Microsoft.Extensions.Caching.Memory;
using System;
using System.Globalization;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.Extensions.Mail.Impl
{
    public class MailEngine : IMailEngine
    {
        readonly IMailTemplateConverter mailTemplateConverter;
        readonly ITemplateLoader templateLoader;
        readonly ITemplateParser templateParser;
        readonly IMemoryCache? memCache;


        public MailEngine(IMailSender mailSender,
                             IMailTemplateConverter mailTemplateConverter,
                             ITemplateLoader templateLoader,
                             ITemplateParser templateParser,
                             IMemoryCache? memCache = null)
        {
            this.Sender = mailSender;
            this.mailTemplateConverter = mailTemplateConverter;
            this.templateLoader = templateLoader;
            this.templateParser = templateParser;
            this.memCache = memCache;
        }


        public IMailSender Sender { get; }


        /// <summary>
        /// Calls the template loader
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="culture"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If content from template loader is null</exception>
        protected virtual async Task<string> Load(string templateName, CultureInfo? culture, CancellationToken cancellationToken)
        {
            var content = await this.templateLoader.Load(templateName, culture, cancellationToken).ConfigureAwait(false);
            if (content == null)
                throw new ArgumentException("Template '{templateName}' not found by loader");

            return content;
        }


        /// <summary>
        /// Will load a template by key (templateName and culture) from memory if memcache is available, otherwise it will call the template loader
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="culture"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task<string> GetTemplate(string templateName, CultureInfo? culture, CancellationToken cancellationToken)
        {
            if (this.memCache == null)
                return this.Load(templateName, culture, cancellationToken);

            var key = $"{templateName}_{culture?.Name}";
            return this.memCache.GetOrCreateAsync<string>(key, _ => this.Load(templateName, culture, cancellationToken));
        }


        /// <summary>
        /// Loads, parses, and converts to a MailMessage
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="args"></param>
        /// <param name="culture"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The fully parsed MailMessage</returns>
        public async Task<MailMessage> Parse(string templateName, object args, CultureInfo? culture = null, CancellationToken cancellationToken = default)
        {
            var content = await this.GetTemplate(templateName, culture, cancellationToken).ConfigureAwait(false);
            content = await this.templateParser.Parse(content, args, culture, cancellationToken).ConfigureAwait(false);
            var mail = await this.mailTemplateConverter.Convert(content, culture, cancellationToken).ConfigureAwait(false);
            return mail;
        }


        /// <summary>
        /// Loads, parses, converts to a MailMessage, and sends it
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="args"></param>
        /// <param name="culture"></param>
        /// <param name="beforeSend"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The fully loaded and parsed MailMessage</returns>
        public async Task<MailMessage> Send(string templateName, object args, CultureInfo? culture = null, Action<MailMessage>? beforeSend = null, CancellationToken cancellationToken = default)
        {
            var mail = await this.Parse(templateName, args, culture, cancellationToken).ConfigureAwait(false);
            beforeSend?.Invoke(mail);

            // could validate email here as the template may not have had address in it
            await this.Sender.Send(mail, cancellationToken).ConfigureAwait(false);
            return mail;
        }
    }
}
