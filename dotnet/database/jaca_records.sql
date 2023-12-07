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
	username NVARCHAR(50) NOT NULL,
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
	CONSTRAINT UQ_email UNIQUE (email_address),
	CONSTRAINT CK_role CHECK (user_role = 'free' OR user_role = 'premium' OR user_role = 'jacapreme')
)

--populate default data
INSERT INTO users (username, first_name, last_name, email_address, password_hash, salt, user_role) VALUES ('user', 'userFirst', 'userLast', 'user@user.com', 'Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','free');
INSERT INTO users (username, first_name, last_name, email_address, password_hash, salt, user_role) VALUES ('jacapreme', 'adminFirst', 'adminLast', 'admin@admin.com', '`YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','jacapreme');

CREATE TABLE friends (
	friend_id int IDENTITY (1, 1) NOT NULL,
	user_id int NOT NULL,
	friends_user_id int NOT NULL,

	CONSTRAINT PK_users_friends PRIMARY KEY (user_id, friends_user_id),
	CONSTRAINT FK_friends_users FOREIGN KEY (user_id) REFERENCES users (user_id),
	CONSTRAINT FK_friends_users_friend FOREIGN KEY (friends_user_id) REFERENCES users (user_id)
)

CREATE TABLE genres (
	genre_id int IDENTITY(1, 1) NOT NULL,
	name NVARCHAR(100) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_genres PRIMARY KEY (genre_id),
	CONSTRAINT UQ_genre_name UNIQUE(name)
)

CREATE TABLE labels (
	label_id int IDENTITY(1, 1) NOT NULL,
	name NVARCHAR(200) NOT NULL,
	url NVARCHAR(500) NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_labels PRIMARY KEY (label_id),
	CONSTRAINT UQ_label_name UNIQUE(name)
)

CREATE TABLE formats(
	format_id int IDENTITY(1, 1) NOT NULL,
	type NVARCHAR(100) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_formats PRIMARY KEY (format_id),
	CONSTRAINT UQ_format_type UNIQUE(type)
)

CREATE TABLE artists (
	artist_id int IDENTITY(1,1) NOT NULL,
	name NVARCHAR(200) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_artists PRIMARY KEY (artist_id),
	CONSTRAINT UQ_artist_name UNIQUE(name),
)

CREATE TABLE records (
	record_id int IDENTITY(1, 1) NOT NULL,
	discogs_id int NOT NULL,
	title NVARCHAR(200) NOT NULL,
	released NVARCHAR(10) NOT NULL,
	country NVARCHAR(10) NULL,
	notes NVARCHAR(2000) NULL,
	url NVARCHAR(500) NULL,
	discogs_date_changed DATETIME NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_records PRIMARY KEY (record_id),
	CONSTRAINT UQ_discogs_id UNIQUE (discogs_id),
)

CREATE TABLE barcodes (
	barcode_id int IDENTITY (1, 1) NOT NULL,
	discogs_id int NOT NULL,
	type NVARCHAR(100) NOT NULL,
	value NVARCHAR(100) NOT NULL,
	description NVARCHAR(500) NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_barcodes PRIMARY KEY (barcode_id),
	CONSTRAINT FK_barcodes_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id)
)

CREATE TABLE images (
	image_id int IDENTITY (1, 1) NOT NULL,
	discogs_id int NOT NULL,
	uri NVARCHAR(500) NULL,
	height int NULL,
	width int NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_images PRIMARY KEY (image_id),
	CONSTRAINT FK_images_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id)
)

CREATE TABLE tracks (
	track_id int IDENTITY(1,1) NOT NULL,
	discogs_id int NOT NULL,
	title NVARCHAR(100) NOT NULL,
	position NVARCHAR(50) NOT NULL,
	duration NVARCHAR(50) NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_tracks PRIMARY KEY (track_id),
	CONSTRAINT FK_tracks_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id)
)

CREATE TABLE records_artists(
	records_artists_id int IDENTITY (1, 1) NOT NULL,
	discogs_id int NOT NULL,
	artist_id int NOT NULL,
	CONSTRAINT PK_records_artists PRIMARY KEY (discogs_id, artist_id),
	CONSTRAINT FK_records_artists_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id),
	CONSTRAINT FK_records_artists_artists FOREIGN KEY (artist_id) REFERENCES artists (artist_id)
)

CREATE TABLE records_extra_artists(
	records_extra_artists_id int IDENTITY (1, 1) NOT NULL,
	discogs_id int NOT NULL,
	extra_artist_id int NOT NULL,
	CONSTRAINT PK_records_extra_artists PRIMARY KEY (discogs_id, extra_artist_id),
	CONSTRAINT FK_records_extra_artists_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id),
	CONSTRAINT FK_records_extra_artists_artists FOREIGN KEY (extra_artist_id) REFERENCES artists (artist_id)
)

CREATE TABLE records_genres(
	records_genres_id int IDENTITY (1, 1) NOT NULL,
	discogs_id int NOT NULL,
	genre_id int NOT NULL,
	CONSTRAINT PK_records_genres PRIMARY KEY (discogs_id, genre_id),
	CONSTRAINT FK_records_genres_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id),
	CONSTRAINT FK_records_genres_genres FOREIGN KEY (genre_id) REFERENCES genres (genre_id)
)

CREATE TABLE records_labels(
	records_labels_id int IDENTITY (1, 1) NOT NULL,
	discogs_id int NOT NULL,
	label_id int NOT NULL,
	CONSTRAINT PK_records_labels PRIMARY KEY (discogs_id, label_id),
	CONSTRAINT FK_records_labels_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id),
	CONSTRAINT FK_records_labels_labels FOREIGN KEY (label_id) REFERENCES labels (label_id)
)

CREATE TABLE records_formats(
	records_formats_id int IDENTITY (1, 1) NOT NULL,
	discogs_id int NOT NULL,
	format_id int NOT NULL,
	CONSTRAINT PK_records_formats PRIMARY KEY (discogs_id, format_id),
	CONSTRAINT FK_records_formats_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id),
	CONSTRAINT FK_records_formats_formats FOREIGN KEY (format_id) REFERENCES formats (format_id)
)


CREATE TABLE libraries (
	library_id int IDENTITY(1, 1) NOT NULL,
	username NVARCHAR(50) NOT NULL,
	discogs_id int NOT NULL,
	notes NVARCHAR(2000) DEFAULT '' NULL,
	quantity int DEFAULT 1 NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_libraries PRIMARY KEY (library_id),
	CONSTRAINT UQ_libraries_username_record UNIQUE (username, discogs_id),
	CONSTRAINT FK_libraries_users FOREIGN KEY (username) REFERENCES users (username),
	CONSTRAINT FK_libraries_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id),
	CONSTRAINT CK_library_quantity CHECK (quantity > -1)

)

CREATE TABLE collections (
	collection_id int IDENTITY(1, 1) NOT NULL,
	username NVARCHAR(50) NOT NULL,
	discogs_id int NULL,
	name NVARCHAR(200) NOT NULL,
	is_private BIT DEFAULT 0 NOT NULL,
	is_active BIT DEFAULT 1 NOT NULL,
	created_date DATETIME DEFAULT getdate() NOT NULL,
	updated_date DATETIME DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_collections PRIMARY KEY (collection_id),
	--CONSTRAINT UQ_collection_name UNIQUE(name),
	CONSTRAINT UQ_collections_libary_record UNIQUE (username, name, discogs_id),
	CONSTRAINT FK_collections_libraries FOREIGN KEY (username) REFERENCES users (username),
	CONSTRAINT FK_collections_records FOREIGN KEY (discogs_id) REFERENCES records (discogs_id)

	
)

GO