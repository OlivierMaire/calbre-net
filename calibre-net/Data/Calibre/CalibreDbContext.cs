﻿// using System;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;

// namespace Calibre_net.Data.Calibre;

// public partial class CalibreDbContext : DbContext
// {
//     public CalibreDbContext(DbContextOptions<CalibreDbContext> options)
//         : base(options)
//     {
//     }

//     public virtual DbSet<Annotation> Annotations { get; set; }

//     public virtual DbSet<AnnotationsDirtied> AnnotationsDirtieds { get; set; }

//     public virtual DbSet<AnnotationsFt> AnnotationsFts { get; set; }

//     public virtual DbSet<AnnotationsFtsConfig> AnnotationsFtsConfigs { get; set; }

//     public virtual DbSet<AnnotationsFtsDatum> AnnotationsFtsData { get; set; }

//     public virtual DbSet<AnnotationsFtsDocsize> AnnotationsFtsDocsizes { get; set; }

//     public virtual DbSet<AnnotationsFtsIdx> AnnotationsFtsIdxes { get; set; }

//     public virtual DbSet<AnnotationsFtsStemmed> AnnotationsFtsStemmeds { get; set; }

//     public virtual DbSet<AnnotationsFtsStemmedConfig> AnnotationsFtsStemmedConfigs { get; set; }

//     public virtual DbSet<AnnotationsFtsStemmedDatum> AnnotationsFtsStemmedData { get; set; }

//     public virtual DbSet<AnnotationsFtsStemmedDocsize> AnnotationsFtsStemmedDocsizes { get; set; }

//     public virtual DbSet<AnnotationsFtsStemmedIdx> AnnotationsFtsStemmedIdxes { get; set; }

//     public virtual DbSet<Author> Authors { get; set; }

//     public virtual DbSet<Book> Books { get; set; }

//     public virtual DbSet<BooksAuthorsLink> BooksAuthorsLinks { get; set; }

//     // public virtual DbSet<BooksCustomColumn1Link> BooksCustomColumn1Links { get; set; }

//     public virtual DbSet<BooksLanguagesLink> BooksLanguagesLinks { get; set; }

//     public virtual DbSet<BooksPluginDatum> BooksPluginData { get; set; }

//     public virtual DbSet<BooksPublishersLink> BooksPublishersLinks { get; set; }

//     public virtual DbSet<BooksRatingsLink> BooksRatingsLinks { get; set; }

//     public virtual DbSet<BooksSeriesLink> BooksSeriesLinks { get; set; }

//     public virtual DbSet<BooksTagsLink> BooksTagsLinks { get; set; }

//     public virtual DbSet<Comment> Comments { get; set; }

//     public virtual DbSet<ConversionOption> ConversionOptions { get; set; }

//     public virtual DbSet<CustomColumn> CustomColumns { get; set; }

//     // public virtual DbSet<CustomColumn1> CustomColumn1 { get; set; }

//     public virtual DbSet<Datum> Data { get; set; }

//     public virtual DbSet<Feed> Feeds { get; set; }

//     public virtual DbSet<Identifier> Identifiers { get; set; }

//     public virtual DbSet<Language> Languages { get; set; }

//     public virtual DbSet<LastReadPosition> LastReadPositions { get; set; }

//     public virtual DbSet<LibraryId> LibraryIds { get; set; }

//     public virtual DbSet<MetadataDirtied> MetadataDirtieds { get; set; }

//     public virtual DbSet<Preference> Preferences { get; set; }

//     public virtual DbSet<Publisher> Publishers { get; set; }

//     public virtual DbSet<Rating> Ratings { get; set; }

//     public virtual DbSet<Series> Series { get; set; }

//     public virtual DbSet<Tag> Tags { get; set; }

//     public virtual DbSet<TagBrowserAuthor> TagBrowserAuthors { get; set; }

//     public virtual DbSet<TagBrowserCustomColumn1> TagBrowserCustomColumn1s { get; set; }

//     public virtual DbSet<TagBrowserPublisher> TagBrowserPublishers { get; set; }

//     public virtual DbSet<TagBrowserRating> TagBrowserRatings { get; set; }

//     public virtual DbSet<TagBrowserTag> TagBrowserTags { get; set; }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<Annotation>(entity =>
//         {
//             entity.ToTable("annotations");

//             entity.HasIndex(e => new { e.Book, e.UserType, e.User, e.Format, e.AnnotType, e.AnnotId }, "IX_annotations_book_user_type_user_format_annot_type_annot_id").IsUnique();

//             entity.HasIndex(e => e.Book, "annot_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.AnnotData).HasColumnName("annot_data");
//             entity.Property(e => e.AnnotId).HasColumnName("annot_id");
//             entity.Property(e => e.AnnotType).HasColumnName("annot_type");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Format)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("format");
//             entity.Property(e => e.SearchableText)
//                 .HasDefaultValueSql("\"\"")
//                 .HasColumnName("searchable_text");
//             entity.Property(e => e.Timestamp).HasColumnName("timestamp");
//             entity.Property(e => e.User).HasColumnName("user");
//             entity.Property(e => e.UserType).HasColumnName("user_type");
//         });

//         modelBuilder.Entity<AnnotationsDirtied>(entity =>
//         {
//             entity.ToTable("annotations_dirtied");

//             entity.HasIndex(e => e.Book, "IX_annotations_dirtied_book").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//         });

//         modelBuilder.Entity<AnnotationsFt>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToTable("annotations_fts");

//             entity.Property(e => e.SearchableText).HasColumnName("searchable_text");
//         });

//         modelBuilder.Entity<AnnotationsFtsConfig>(entity =>
//         {
//             entity.HasKey(e => e.K);

//             entity.ToTable("annotations_fts_config");

//             entity.Property(e => e.K).HasColumnName("k");
//             entity.Property(e => e.V).HasColumnName("v");
//         });

//         modelBuilder.Entity<AnnotationsFtsDatum>(entity =>
//         {
//             entity.ToTable("annotations_fts_data");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Block).HasColumnName("block");
//         });

//         modelBuilder.Entity<AnnotationsFtsDocsize>(entity =>
//         {
//             entity.ToTable("annotations_fts_docsize");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Sz).HasColumnName("sz");
//         });

//         modelBuilder.Entity<AnnotationsFtsIdx>(entity =>
//         {
//             entity.HasKey(e => new { e.Segid, e.Term });

//             entity.ToTable("annotations_fts_idx");

//             entity.Property(e => e.Segid).HasColumnName("segid");
//             entity.Property(e => e.Term).HasColumnName("term");
//             entity.Property(e => e.Pgno).HasColumnName("pgno");
//         });

//         modelBuilder.Entity<AnnotationsFtsStemmed>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToTable("annotations_fts_stemmed");

//             entity.Property(e => e.SearchableText).HasColumnName("searchable_text");
//         });

//         modelBuilder.Entity<AnnotationsFtsStemmedConfig>(entity =>
//         {
//             entity.HasKey(e => e.K);

//             entity.ToTable("annotations_fts_stemmed_config");

//             entity.Property(e => e.K).HasColumnName("k");
//             entity.Property(e => e.V).HasColumnName("v");
//         });

//         modelBuilder.Entity<AnnotationsFtsStemmedDatum>(entity =>
//         {
//             entity.ToTable("annotations_fts_stemmed_data");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Block).HasColumnName("block");
//         });

//         modelBuilder.Entity<AnnotationsFtsStemmedDocsize>(entity =>
//         {
//             entity.ToTable("annotations_fts_stemmed_docsize");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Sz).HasColumnName("sz");
//         });

//         modelBuilder.Entity<AnnotationsFtsStemmedIdx>(entity =>
//         {
//             entity.HasKey(e => new { e.Segid, e.Term });

//             entity.ToTable("annotations_fts_stemmed_idx");

//             entity.Property(e => e.Segid).HasColumnName("segid");
//             entity.Property(e => e.Term).HasColumnName("term");
//             entity.Property(e => e.Pgno).HasColumnName("pgno");
//         });

//         modelBuilder.Entity<Author>(entity =>
//         {
//             entity.ToTable("authors");

//             entity.HasIndex(e => e.Name, "IX_authors_name").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Link)
//                 .HasDefaultValueSql("\"\"")
//                 .HasColumnName("link");
//             entity.Property(e => e.Name)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("name");
//             entity.Property(e => e.Sort)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("sort");
//         });

//         modelBuilder.Entity<Book>(entity =>
//         {
//             entity.ToTable("books");

//             entity.HasIndex(e => e.AuthorSort, "authors_idx");

//             entity.HasIndex(e => e.Sort, "books_idx");

//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.AuthorSort)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("author_sort");
//             entity.Property(e => e.Flags)
//                 .HasDefaultValue(1)
//                 .HasColumnName("flags");
//             entity.Property(e => e.HasCover)
//                 .HasDefaultValue(false)
//                 .HasColumnType("BOOL")
//                 .HasColumnName("has_cover");
//             entity.Property(e => e.Isbn)
//                 .HasDefaultValueSql("\"\"")
//                 .UseCollation("NOCASE")
//                 .HasColumnName("isbn");
//             entity.Property(e => e.LastModified)
//                 .HasDefaultValueSql("\"2000-01-01 00:00:00+00:00\"")
//                 .HasColumnType("TIMESTAMP")
//                 .HasColumnName("last_modified");
//             entity.Property(e => e.Lccn)
//                 .HasDefaultValueSql("\"\"")
//                 .UseCollation("NOCASE")
//                 .HasColumnName("lccn");
//             entity.Property(e => e.Path)
//                 .HasDefaultValueSql("\"\"")
//                 .HasColumnName("path");
//             entity.Property(e => e.Pubdate)
//                 .HasDefaultValueSql("CURRENT_TIMESTAMP")
//                 .HasColumnType("TIMESTAMP")
//                 .HasColumnName("pubdate");
//             entity.Property(e => e.SeriesIndex)
//                 .HasDefaultValue(1.0)
//                 .HasColumnName("series_index");
//             entity.Property(e => e.Sort)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("sort");
//             entity.Property(e => e.Timestamp)
//                 .HasDefaultValueSql("CURRENT_TIMESTAMP")
//                 .HasColumnType("TIMESTAMP")
//                 .HasColumnName("timestamp");
//             entity.Property(e => e.Title)
//                 .HasDefaultValue("Unknown")
//                 .UseCollation("NOCASE")
//                 .HasColumnName("title");
//             entity.Property(e => e.Uuid).HasColumnName("uuid");
//         });

//         modelBuilder.Entity<BooksAuthorsLink>(entity =>
//         {
//             entity.ToTable("books_authors_link");

//             entity.HasIndex(e => new { e.Book, e.Author }, "IX_books_authors_link_book_author").IsUnique();

//             entity.HasIndex(e => e.Author, "books_authors_link_aidx");

//             entity.HasIndex(e => e.Book, "books_authors_link_bidx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Author).HasColumnName("author");
//             entity.Property(e => e.Book).HasColumnName("book");
//         });

//         // modelBuilder.Entity<BooksCustomColumn1Link>(entity =>
//         // {
//         //     entity.ToTable("books_custom_column_1_link");

//         //     entity.HasIndex(e => new { e.Book, e.Value }, "IX_books_custom_column_1_link_book_value").IsUnique();

//         //     entity.HasIndex(e => e.Value, "books_custom_column_1_link_aidx");

//         //     entity.HasIndex(e => e.Book, "books_custom_column_1_link_bidx");

//         //     entity.Property(e => e.Id).HasColumnName("id");
//         //     entity.Property(e => e.Book).HasColumnName("book");
//         //     entity.Property(e => e.Value).HasColumnName("value");
//         // });

//         modelBuilder.Entity<BooksLanguagesLink>(entity =>
//         {
//             entity.ToTable("books_languages_link");

//             entity.HasIndex(e => new { e.Book, e.LangCode }, "IX_books_languages_link_book_lang_code").IsUnique();

//             entity.HasIndex(e => e.LangCode, "books_languages_link_aidx");

//             entity.HasIndex(e => e.Book, "books_languages_link_bidx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.ItemOrder).HasColumnName("item_order");
//             entity.Property(e => e.LangCode).HasColumnName("lang_code");
//         });

//         modelBuilder.Entity<BooksPluginDatum>(entity =>
//         {
//             entity.ToTable("books_plugin_data");

//             entity.HasIndex(e => new { e.Book, e.Name }, "IX_books_plugin_data_book_name").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Name).HasColumnName("name");
//             entity.Property(e => e.Val).HasColumnName("val");
//         });

//         modelBuilder.Entity<BooksPublishersLink>(entity =>
//         {
//             entity.ToTable("books_publishers_link");

//             entity.HasIndex(e => e.Book, "IX_books_publishers_link_book").IsUnique();

//             entity.HasIndex(e => e.Publisher, "books_publishers_link_aidx");

//             entity.HasIndex(e => e.Book, "books_publishers_link_bidx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Publisher).HasColumnName("publisher");
//         });

//         modelBuilder.Entity<BooksRatingsLink>(entity =>
//         {
//             entity.ToTable("books_ratings_link");

//             entity.HasIndex(e => new { e.Book, e.Rating }, "IX_books_ratings_link_book_rating").IsUnique();

//             entity.HasIndex(e => e.Rating, "books_ratings_link_aidx");

//             entity.HasIndex(e => e.Book, "books_ratings_link_bidx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Rating).HasColumnName("rating");
//         });

//         modelBuilder.Entity<BooksSeriesLink>(entity =>
//         {
//             entity.ToTable("books_series_link");

//             entity.HasIndex(e => e.Book, "IX_books_series_link_book").IsUnique();

//             entity.HasIndex(e => e.Series, "books_series_link_aidx");

//             entity.HasIndex(e => e.Book, "books_series_link_bidx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Series).HasColumnName("series");
//         });

//         modelBuilder.Entity<BooksTagsLink>(entity =>
//         {
//             entity.ToTable("books_tags_link");

//             entity.HasIndex(e => new { e.Book, e.Tag }, "IX_books_tags_link_book_tag").IsUnique();

//             entity.HasIndex(e => e.Tag, "books_tags_link_aidx");

//             entity.HasIndex(e => e.Book, "books_tags_link_bidx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Tag).HasColumnName("tag");
//         });

//         modelBuilder.Entity<Comment>(entity =>
//         {
//             entity.ToTable("comments");

//             entity.HasIndex(e => e.Book, "IX_comments_book").IsUnique();

//             entity.HasIndex(e => e.Book, "comments_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Text)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("text");
//         });

//         modelBuilder.Entity<ConversionOption>(entity =>
//         {
//             entity.ToTable("conversion_options");

//             entity.HasIndex(e => new { e.Format, e.Book }, "IX_conversion_options_format_book").IsUnique();

//             entity.HasIndex(e => e.Format, "conversion_options_idx_a");

//             entity.HasIndex(e => e.Book, "conversion_options_idx_b");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Data).HasColumnName("data");
//             entity.Property(e => e.Format)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("format");
//         });

//         modelBuilder.Entity<CustomColumn>(entity =>
//         {
//             entity.ToTable("custom_columns");

//             entity.HasIndex(e => e.Label, "IX_custom_columns_label").IsUnique();

//             entity.HasIndex(e => e.Label, "custom_columns_idx");

//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Datatype).HasColumnName("datatype");
//             entity.Property(e => e.Display)
//                 .HasDefaultValueSql("\"{}\"")
//                 .HasColumnName("display");
//             entity.Property(e => e.Editable)
//                 .HasDefaultValue(true)
//                 .HasColumnType("BOOL")
//                 .HasColumnName("editable");
//             entity.Property(e => e.IsMultiple)
//                 .HasColumnType("BOOL")
//                 .HasColumnName("is_multiple");
//             entity.Property(e => e.Label).HasColumnName("label");
//             entity.Property(e => e.MarkForDelete)
//                 .HasColumnType("BOOL")
//                 .HasColumnName("mark_for_delete");
//             entity.Property(e => e.Name).HasColumnName("name");
//             entity.Property(e => e.Normalized)
//                 .HasColumnType("BOOL")
//                 .HasColumnName("normalized");
//         });

//         // modelBuilder.Entity<CustomColumn1>(entity =>
//         // {
//         //     entity.ToTable("custom_column_1");

//         //     entity.HasIndex(e => e.Value, "IX_custom_column_1_value").IsUnique();

//         //     entity.HasIndex(e => e.Value, "custom_column_1_idx");

//         //     entity.Property(e => e.Id).HasColumnName("id");
//         //     entity.Property(e => e.Link)
//         //         .HasDefaultValue("")
//         //         .HasColumnName("link");
//         //     entity.Property(e => e.Value)
//         //         .UseCollation("NOCASE")
//         //         .HasColumnName("value");
//         // });

//         modelBuilder.Entity<Datum>(entity =>
//         {
//             entity.ToTable("data");

//             entity.HasIndex(e => new { e.Book, e.Format }, "IX_data_book_format").IsUnique();

//             entity.HasIndex(e => e.Book, "data_idx");

//             entity.HasIndex(e => e.Format, "formats_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Format)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("format");
//             entity.Property(e => e.Name).HasColumnName("name");
//             entity.Property(e => e.UncompressedSize).HasColumnName("uncompressed_size");
//         });

//         modelBuilder.Entity<Feed>(entity =>
//         {
//             entity.ToTable("feeds");

//             entity.HasIndex(e => e.Title, "IX_feeds_title").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Script).HasColumnName("script");
//             entity.Property(e => e.Title).HasColumnName("title");
//         });

//         modelBuilder.Entity<Identifier>(entity =>
//         {
//             entity.ToTable("identifiers");

//             entity.HasIndex(e => new { e.Book, e.Type }, "IX_identifiers_book_type").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Type)
//                 .HasDefaultValueSql("\"isbn\"")
//                 .UseCollation("NOCASE")
//                 .HasColumnName("type");
//             entity.Property(e => e.Val)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("val");
//         });

//         modelBuilder.Entity<Language>(entity =>
//         {
//             entity.ToTable("languages");

//             entity.HasIndex(e => e.LangCode, "IX_languages_lang_code").IsUnique();

//             entity.HasIndex(e => e.LangCode, "languages_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.LangCode)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("lang_code");
//             entity.Property(e => e.Link)
//                 .HasDefaultValue("")
//                 .HasColumnName("link");
//         });

//         modelBuilder.Entity<LastReadPosition>(entity =>
//         {
//             entity.ToTable("last_read_positions");

//             entity.HasIndex(e => new { e.User, e.Device, e.Book, e.Format }, "IX_last_read_positions_user_device_book_format").IsUnique();

//             entity.HasIndex(e => e.Book, "lrp_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//             entity.Property(e => e.Cfi).HasColumnName("cfi");
//             entity.Property(e => e.Device).HasColumnName("device");
//             entity.Property(e => e.Epoch).HasColumnName("epoch");
//             entity.Property(e => e.Format)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("format");
//             entity.Property(e => e.PosFrac).HasColumnName("pos_frac");
//             entity.Property(e => e.User).HasColumnName("user");
//         });

//         modelBuilder.Entity<LibraryId>(entity =>
//         {
//             entity.ToTable("library_id");

//             entity.HasIndex(e => e.Uuid, "IX_library_id_uuid").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Uuid).HasColumnName("uuid");
//         });

//         modelBuilder.Entity<MetadataDirtied>(entity =>
//         {
//             entity.ToTable("metadata_dirtied");

//             entity.HasIndex(e => e.Book, "IX_metadata_dirtied_book").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Book).HasColumnName("book");
//         });

//         modelBuilder.Entity<Preference>(entity =>
//         {
//             entity.ToTable("preferences");

//             entity.HasIndex(e => e.Key, "IX_preferences_key").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Key).HasColumnName("key");
//             entity.Property(e => e.Val).HasColumnName("val");
//         });

//         modelBuilder.Entity<Publisher>(entity =>
//         {
//             entity.ToTable("publishers");

//             entity.HasIndex(e => e.Name, "IX_publishers_name").IsUnique();

//             entity.HasIndex(e => e.Name, "publishers_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Link)
//                 .HasDefaultValue("")
//                 .HasColumnName("link");
//             entity.Property(e => e.Name)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("name");
//             entity.Property(e => e.Sort)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("sort");
//         });

//         modelBuilder.Entity<Rating>(entity =>
//         {
//             entity.ToTable("ratings");

//             entity.HasIndex(e => e.RatingValue, "IX_ratings_rating").IsUnique();

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Link)
//                 .HasDefaultValue("")
//                 .HasColumnName("link");
//             entity.Property(e => e.RatingValue).HasColumnName("rating");
//         });

//         modelBuilder.Entity<Series>(entity =>
//         {
//             entity.ToTable("series");

//             entity.HasIndex(e => e.Name, "IX_series_name").IsUnique();

//             entity.HasIndex(e => e.Name, "series_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Link)
//                 .HasDefaultValue("")
//                 .HasColumnName("link");
//             entity.Property(e => e.Name)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("name");
//             entity.Property(e => e.Sort)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("sort");
//         });

//         modelBuilder.Entity<Tag>(entity =>
//         {
//             entity.ToTable("tags");

//             entity.HasIndex(e => e.Name, "IX_tags_name").IsUnique();

//             entity.HasIndex(e => e.Name, "tags_idx");

//             entity.Property(e => e.Id)
//                 .ValueGeneratedNever()
//                 .HasColumnName("id");
//             entity.Property(e => e.Link)
//                 .HasDefaultValue("")
//                 .HasColumnName("link");
//             entity.Property(e => e.Name)
//                 .UseCollation("NOCASE")
//                 .HasColumnName("name");
//         });

//         modelBuilder.Entity<TagBrowserAuthor>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToView("tag_browser_authors");

//             entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
//             entity.Property(e => e.Count).HasColumnName("count");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Name).HasColumnName("name");
//             entity.Property(e => e.Sort).HasColumnName("sort");
//         });

//         modelBuilder.Entity<TagBrowserCustomColumn1>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToView("tag_browser_custom_column_1");

//             entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
//             entity.Property(e => e.Count).HasColumnName("count");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Sort).HasColumnName("sort");
//             entity.Property(e => e.Value).HasColumnName("value");
//         });

//         modelBuilder.Entity<TagBrowserPublisher>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToView("tag_browser_publishers");

//             entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
//             entity.Property(e => e.Count).HasColumnName("count");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Name).HasColumnName("name");
//             entity.Property(e => e.Sort).HasColumnName("sort");
//         });

//         modelBuilder.Entity<TagBrowserRating>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToView("tag_browser_ratings");

//             entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
//             entity.Property(e => e.Count).HasColumnName("count");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Rating).HasColumnName("rating");
//             entity.Property(e => e.Sort).HasColumnName("sort");
//         });

//         modelBuilder.Entity<TagBrowserTag>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToView("tag_browser_tags");

//             entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
//             entity.Property(e => e.Count).HasColumnName("count");
//             entity.Property(e => e.Id).HasColumnName("id");
//             entity.Property(e => e.Name).HasColumnName("name");
//             entity.Property(e => e.Sort).HasColumnName("sort");
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }
