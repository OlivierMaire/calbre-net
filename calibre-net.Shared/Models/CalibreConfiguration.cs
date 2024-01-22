using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace calibre_net.Shared.Models;

// [JsonPropertyName("calibre")]
public class CalibreConfiguration
{
    public static string SectionName = "calibre";

    [JsonPropertyName("database")]
    [ConfigurationKeyName("database")]
    public DatabaseConfiguration? Database { get; set; } = new();
    [JsonPropertyName("basic")]
    [ConfigurationKeyName("basic")]
    public BasicConfiguration? Basic { get; set; } = new();
    [JsonPropertyName("ui")]
    [ConfigurationKeyName("ui")]
    public UiConfiguration? UI { get; set; } = new();
}

public class DatabaseConfiguration
{
    [JsonPropertyName("location")]
    public string? Location { get; set; } = ".";
}

public class BasicConfiguration
{
    [JsonPropertyName("server")]
    [ConfigurationKeyName("server")]
    public ServerConfiguration? Server { get; set; } = new();
    [JsonPropertyName("logfile")]
    [ConfigurationKeyName("logfile")]
    public LogfileConfiguration? Logfile { get; set; } = new();
    [JsonPropertyName("feature")]
    [ConfigurationKeyName("feature")]
    public FeatureConfiguration? Feature { get; set; } = new();
    [JsonPropertyName("external")]
    [ConfigurationKeyName("external")]
    public ExternalConfiguration? External { get; set; } = new();
    [JsonPropertyName("security")]
    [ConfigurationKeyName("security")]
    public SecurityConfiguration? Security { get; set; } = new();


    public class ServerConfiguration
    {
        [JsonPropertyName("port")]
        [ConfigurationKeyName("port")]
        public int? Port { get; set; }
        [JsonPropertyName("ssl_certfile_path")]
        [ConfigurationKeyName("ssl_certfile_path")]
        public string? SslCertfilePath { get; set; } = string.Empty;
        [JsonPropertyName("ssl_keyfile_path")]
        [ConfigurationKeyName("ssl_keyfile_path")]
        public string? SslKeyfilePath { get; set; } = string.Empty;
        [JsonPropertyName("update_channel")]
        [ConfigurationKeyName("update_channel")]
        public string? UpdateChannel { get; set; } = "stable";
        [JsonPropertyName("trusted_hosts")]
        [ConfigurationKeyName("trusted_hosts")]
        public string? TrustedHosts { get; set; } = string.Empty;
    }

    public class LogfileConfiguration
    {

        [JsonPropertyName("log_level")]
        [ConfigurationKeyName("log_level")]
        public string? LogLevel { get; set; } = "info";
        [JsonPropertyName("logfile_path")]
        [ConfigurationKeyName("logfile_path")]
        public string? LogfilePath { get; set; } = string.Empty;

        [JsonPropertyName("accesslog_enabled")]
        [ConfigurationKeyName("accesslog_enabled")]
        public bool? AccessLogEnabled { get; set; } = false;

        [JsonPropertyName("accesslog_path")]
        [ConfigurationKeyName("accesslog_path")]
        public string? AccessLogPath { get; set; } = string.Empty;
    }

    public class FeatureConfiguration
    {
        [JsonPropertyName("convert_non_english")]
        [ConfigurationKeyName("convert_non_english")]
        public bool? ConvertNonEnglish { get; set; } = false;
        [JsonPropertyName("upload_enabled")]
        [ConfigurationKeyName("upload_enabled")]
        public bool? UploadEnabled { get; set; } = false;
        [JsonPropertyName("upload_formats_allowed")]
        [ConfigurationKeyName("upload_formats_allowed")]
        public string? UploadFormatsAllowed { get; set; } = string.Empty;
        [JsonPropertyName("anonymous_enabled")]
        [ConfigurationKeyName("anonymous_enabled")]
        public bool? AnonymousEnabled { get; set; } = false;
        [JsonPropertyName("registration_enabled")]
        [ConfigurationKeyName("registration_enabled")]
        public bool? RegistrationEnabled { get; set; } = false;
        [JsonPropertyName("email_as_username_enabled")]
        [ConfigurationKeyName("email_as_username_enabled")]
        public bool? UseEmailAsUsername { get; set; } = false;
        [JsonPropertyName("magiclink_enabled")]
        [ConfigurationKeyName("magiclink_enabled")]
        public bool? MagicLinkEnabled { get; set; } = false;
        [JsonPropertyName("reverse_proxy_auth_enabled")]
        [ConfigurationKeyName("reverse_proxy_auth_enabled")]
        public bool? ReverseProxyAuthEnabled { get; set; } = false;
        [JsonPropertyName("reverse_proxy_header")]
        [ConfigurationKeyName("reverse_proxy_header")]
        public string? ReverseProxyHeaderName { get; set; } = string.Empty;
        [JsonPropertyName("oauth_google_clientid")]
        [ConfigurationKeyName("oauth_google_clientid")]
        public string? OAuthGoogleClientId { get; set; } = string.Empty;
        [JsonPropertyName("oauth_google_secret")]
        [ConfigurationKeyName("oauth_google_secret")]
        public string? OAuthGoogleClientSecret { get; set; } = string.Empty;
        [JsonPropertyName("oauth_github_clientid")]
        [ConfigurationKeyName("oauth_github_clientid")]
        public string? OAuthGitHubClientId { get; set; } = string.Empty;
        [JsonPropertyName("oauth_github_secret")]
        [ConfigurationKeyName("oauth_github_secret")]
        public string? OAuthGitHubClientSecret { get; set; } = string.Empty;
    }

    public class ExternalConfiguration
    {

        [JsonPropertyName("calibre_ebook_converter_path")]
        [ConfigurationKeyName("calibre_ebook_converter_path")]
        public string? CalibreEbookConverterPath { get; set; } = string.Empty;
        [JsonPropertyName("calibre_ebook_converter_settings")]
        [ConfigurationKeyName("calibre_ebook_converter_settings")]
        public string? CalibreEbookConverterSettings { get; set; } = string.Empty;
        [JsonPropertyName("kepubify_ebook_converter_path")]
        [ConfigurationKeyName("kepubify_ebook_converter_path")]
        public string? KepubifyEbookConverterPath { get; set; } = string.Empty;
        [JsonPropertyName("unrar_path")]
        [ConfigurationKeyName("unrar_path")]
        public string? UnrarPath { get; set; } = string.Empty;
    }

    public class SecurityConfiguration
    {
        [JsonPropertyName("failed_login_limit")]
        [ConfigurationKeyName("failed_login_limit")]
        public int? FailedLoginAttemptLimit { get; set; } = 5;
        [JsonPropertyName("enforce_password_policy")]
        [ConfigurationKeyName("enforce_password_policy")]
        public bool? EnforcePasswordPolicy { get; set; } = true;
        [JsonPropertyName("min_password_length")]
        [ConfigurationKeyName("min_password_length")]
        public int? MinPasswordLength { get; set; } = 8;
        [JsonPropertyName("enforce_password_number")]
        [ConfigurationKeyName("enforce_password_number")]
        public bool? EnforcePasswordNumber { get; set; } = true;
        [JsonPropertyName("enforce_password_lowercase")]
        [ConfigurationKeyName("enforce_password_lowercase")]
        public bool? EnforcePasswordLowercase { get; set; } = true;
        [JsonPropertyName("enforce_password_uppercase")]
        [ConfigurationKeyName("enforce_password_uppercase")]
        public bool? EnforcePasswordUppercase { get; set; } = true;
        [JsonPropertyName("enforce_password_special")]
        [ConfigurationKeyName("enforce_password_special")]
        public bool? EnforcePasswordSpecial { get; set; } = true;
        [JsonPropertyName("passkey_allowed")]
        [ConfigurationKeyName("passkey_allowed")]
        public bool? PasskeyAllowed { get; set; } = true;
        [JsonPropertyName("mfa_allowed")]
        [ConfigurationKeyName("mfa_allowed")]
        public bool? MfaAllowed { get; set; } = true;
    }
}


public class UiConfiguration
{
    [JsonPropertyName("view")]
    [ConfigurationKeyName("view")]
    public ViewConfiguration? View { get; set; } = new();

    [JsonPropertyName("default_settings")]
    [ConfigurationKeyName("default_settings")]
    public DefaultSettingsConfiguration? DefaultSettings { get; set; } = new();
    [JsonPropertyName("default_visibilities")]
    [ConfigurationKeyName("default_visibilities")]
    public DefaultVisibilitiesConfiguration? DefaultVisibilities { get; set; } = new();

    public class ViewConfiguration
    {
        [JsonPropertyName("site_title")]
        [ConfigurationKeyName("site_title")]
        public string? SiteTitle { get; set; } = "Calibre.net";

        [JsonPropertyName("books_per_page")]
        [ConfigurationKeyName("books_per_page")]
        public int? BooksPerPage { get; set; } = 60;

        [JsonPropertyName("random_book_dispplay")]
        [ConfigurationKeyName("random_book_display")]
        public int? RandomBooksDisplayNb { get; set; } = 4;

        [JsonPropertyName("nb_author_before_hide")]
        [ConfigurationKeyName("nb_author_before_hide")]
        public int? NbAuthorDisplayedBeforeHiding { get; set; } = 0;

        [JsonPropertyName("theme")]
        [ConfigurationKeyName("theme")]
        public string? ThemeName { get; set; } = "Standard Theme";
        
        [JsonPropertyName("ignore_column_regex")]
        [ConfigurationKeyName("ignore_column_regex")]
        public string? IgnoreColumnRegex { get; set; } = string.Empty;
        [JsonPropertyName("read_status_column")]
        [ConfigurationKeyName("read_status_column")]
        public string? ReadStatusColumn { get; set; } = string.Empty;
        [JsonPropertyName("view_restriction_column")]
        [ConfigurationKeyName("view_restriction_column")]
        public string? ViewRestrictionColumn { get; set; } = string.Empty;
        [JsonPropertyName("title_sort_regex")]
        [ConfigurationKeyName("title_sort_regex")]
        public string? TitleSortRegex { get; set; } = "^(A|The|An|Der|Die|Das|Den|Ein|Eine|Einen|Dem|Des|Einem|Eines)\\s+";
    }

    public class DefaultSettingsConfiguration{

    }

    public class DefaultVisibilitiesConfiguration{
        
    }



}