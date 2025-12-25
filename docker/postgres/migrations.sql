CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "Categories" (
        "Id" uuid NOT NULL,
        "Name" varchar(200) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "Contacts" (
        "Id" uuid NOT NULL,
        "Name" varchar(200) NOT NULL,
        "Email" varchar(254) NOT NULL,
        "Message" varchar(1000) NOT NULL,
        "Received" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Contacts" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "PageViews" (
        "Id" uuid NOT NULL,
        "PageId" uuid NOT NULL,
        "Date" date NOT NULL,
        "TotalViews" integer NOT NULL DEFAULT 0,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_PageViews" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "Posts" (
        "Id" uuid NOT NULL,
        "Title" varchar(200) NOT NULL,
        "UrlSlug" varchar(200) NOT NULL,
        "UrlMainImage" varchar(200) NOT NULL,
        "Content" text NOT NULL,
        "Summary" varchar(1000) NOT NULL,
        "Status" integer NOT NULL,
        "MetaTitle" varchar(200),
        "MetaDescription" varchar(500),
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Posts" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "Tags" (
        "Id" uuid NOT NULL,
        "Name" varchar(200) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Tags" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "Comments" (
        "Id" uuid NOT NULL,
        "UserName" varchar(200) NOT NULL,
        "Content" varchar(200) NOT NULL,
        "CommentId" uuid,
        "PostId" uuid,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Comments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Comments_Comments_CommentId" FOREIGN KEY ("CommentId") REFERENCES "Comments" ("Id"),
        CONSTRAINT "FK_Comments_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "PostCategories" (
        "PostId" uuid NOT NULL,
        "CategoryId" uuid NOT NULL,
        CONSTRAINT "PK_PostCategories" PRIMARY KEY ("PostId", "CategoryId"),
        CONSTRAINT "FK_PostCategories_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_PostCategories_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE TABLE "PostTags" (
        "PostId" uuid NOT NULL,
        "TagId" uuid NOT NULL,
        CONSTRAINT "PK_PostTags" PRIMARY KEY ("PostId", "TagId"),
        CONSTRAINT "FK_PostTags_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_PostTags_Tags_TagId" FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE INDEX "IX_Comments_CommentId" ON "Comments" ("CommentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE INDEX "IX_Comments_PostId" ON "Comments" ("PostId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE UNIQUE INDEX "IX_PageViews_PageId_Date" ON "PageViews" ("PageId", "Date");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE INDEX "IX_PostCategories_CategoryId" ON "PostCategories" ("CategoryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE UNIQUE INDEX "IX_Posts_UrlSlug" ON "Posts" ("UrlSlug");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    CREATE INDEX "IX_PostTags_TagId" ON "PostTags" ("TagId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251224014005_DannylloSouzaBlog') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251224014005_DannylloSouzaBlog', '8.0.22');
    END IF;
END $EF$;
COMMIT;

