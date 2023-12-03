USE master
GO 

--drop database if it exists
IF DB_ID('jaca_records') IS NOT NULL
BEGIN
	ALTER DATABASE jaca_records SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE jaca_records;
END

CREATE DATABASE jaca_records
GO

USE jaca_records
GO

--create tables
CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username varchar(50) NOT NULL,
	first_name NVARCHAR(50) NOT NULL,
	last_name NVARCHAR(200) NOT NULL,
	email_address NVARCHAR(200) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	last_login DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_user PRIMARY KEY (user_id),
	CONSTRAINT UQ_username UNIQUE (username),
	CONSTRAINT CK_role CHECK (user_role = 'free' OR user_role = 'premium' OR user_role = 'jacapreme')
)

--populate default data
INSERT INTO users (username, first_name, last_name, email_address, password_hash, salt, user_role) VALUES ('user', 'userFirst', 'userLast', 'user@user.com', 'Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','free');
INSERT INTO users (username, first_name, last_name, email_address, password_hash, salt, user_role) VALUES ('jacapreme', 'adminFirst', 'adminLast', 'admin@admin.com', '`YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','jacapreme');

CREATE TABLE friends (
	friend_id int IDENTITY (1, 1) NOT NULL,
	user_id int NOT NULL,

	CONSTRAINT PK_friend PRIMARY KEY (friend_id),
	CONSTRAINT FK_friends_users FOREIGN KEY (user_id) REFERENCES users (user_id)
)

CREATE TABLE genres (
	genre_id int IDENTITY(1, 1) NOT NULL,
	name NVARCHAR(50) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_genres PRIMARY KEY (genre_id),
	CONSTRAINT UQ_genre_name UNIQUE(name)
)

CREATE TABLE labels (
	label_id int IDENTITY(1, 1) NOT NULL,
	name NVARCHAR(100) NOT NULL,
	url NVARCHAR(500) NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_labels PRIMARY KEY (label_id),
	CONSTRAINT UQ_label_name UNIQUE(name)
)

CREATE TABLE formats(
	format_id int IDENTITY(1, 1) NOT NULL,
	size NVARCHAR(10) NOT NULL,
	type NVARCHAR(10) NOT NULL,
	rpm NVARCHAR(10) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_formats PRIMARY KEY (format_id),
	CONSTRAINT UQ_format_type UNIQUE(type)
)

CREATE TABLE artists (
	artist_id int IDENTITY(1,1) NOT NULL,
	name NVARCHAR(100) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_artists PRIMARY KEY (artist_id),
	CONSTRAINT UQ_artist_name UNIQUE(name),
)

CREATE TABLE tracks (
	track_id int IDENTITY(1,1) NOT NULL,
	artist_id int NOT NULL,
	title NVARCHAR(100) NOT NULL,
	position NVARCHAR(10) NOT NULL,
	duration NVARCHAR(10) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_tracks PRIMARY KEY (track_id),
	CONSTRAINT FK_tracks_artists FOREIGN KEY (artist_id) REFERENCES artists (artist_id)
)

CREATE TABLE records (
	record_id int IDENTITY(1, 1) NOT NULL,
	discogs_id int NOT NULL,
	artist_id int NOT NULL,
	genre_id int NOT NULL,
	label_id int NOT NULL,
	format_id int NOT NULL,
	barcode NVARCHAR(20) NOT NULL,
	extra_artist_id int NULL,
	country NVARCHAR(50) NULL,
	img_url NVARCHAR(500) NULL,
	released NVARCHAR(10) NOT NULL,
	url NVARCHAR(500) NULL,
	notes NVARCHAR(1000) NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_records PRIMARY KEY (record_id),
	CONSTRAINT UQ_discogs_id UNIQUE (discogs_id),
	CONSTRAINT FK_records_artists FOREIGN KEY (artist_id) REFERENCES artists (artist_id),
	CONSTRAINT FK_records_genres FOREIGN KEY (genre_id) REFERENCES genres (genre_id),
	CONSTRAINT FK_records_labels FOREIGN KEY (label_id) REFERENCES labels (label_id),
	CONSTRAINT FK_records_format FOREIGN KEY (format_id) REFERENCES formats (format_id),
	CONSTRAINT FK_records_extra_artists FOREIGN KEY (extra_artist_id) REFERENCES artists (artist_id)
)

CREATE TABLE libraries (
	library_id int IDENTITY(1, 1) NOT NULL,
	user_id int NOT NULL,
	record_id int NOT NULL,
	notes NVARCHAR(1000) DEFAULT '' NULL,
	quantity int DEFAULT 1 NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_libraries PRIMARY KEY (library_id),
	CONSTRAINT FK_libraries_users FOREIGN KEY (user_id) REFERENCES users (user_id),
	CONSTRAINT FK_libraries_records FOREIGN KEY (record_id) REFERENCES records (record_id)

)

CREATE TABLE collections (
	collection_id int IDENTITY(1, 1) NOT NULL,
	library_id int NOT NULL,
	record_id int NOT NULL,
	name NVARCHAR(50) NOT NULL,
	is_private BIT DEFAULT 0 NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_collections PRIMARY KEY (collection_id),
	CONSTRAINT UQ_collection_name UNIQUE(name),
	CONSTRAINT FK_collections_libraries FOREIGN KEY (library_id) REFERENCES libraries (library_id),
	CONSTRAINT FK_collections_records FOREIGN KEY (record_id) REFERENCES records (record_id)

	
)

GO