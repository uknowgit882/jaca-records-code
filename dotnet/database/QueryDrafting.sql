SELECT * 
FROM records
LEFT JOIN records_artists ON records.discogs_id = records_artists.discogs_id
LEFT JOIN artists ON records_artists.artist_id = artists.artist_id

-- JOIN records_extra_artists ON records.discogs_id = records_extra_artists.discogs_id
-- JOIN artists AS extra_artists ON records_extra_artists.extra_artist_id = extra_artists.artist_id

-- JOIN images ON records.discogs_id = images.discogs_id

LEFT JOIN records_labels ON records.discogs_id = records_labels.discogs_id
LEFT JOIN labels ON records_labels.label_id = labels.label_id

-- JOIN tracks ON records.discogs_id = tracks.discogs_id

LEFT JOIN barcodes ON records.discogs_id = barcodes.discogs_id

-- JOIN records_formats ON records.discogs_id = records_formats.discogs_id
-- JOIN formats ON records_formats.format_id = formats.format_id 

LEFT JOIN records_genres ON records.discogs_id = records_genres.discogs_id
LEFT JOIN genres ON records_genres.genre_id = genres.genre_id

BEGIN TRANSACTION
UPDATE records 
SET is_active = 0
WHERE discogs_id = 249504

SELECT *
FROM records
ROLLBACK

BEGIN TRANSACTION
UPDATE records 
SET country = 'USAA', notes = 'who cares', released = '2023', title = 'Queenie 2, the Queenening', url = '', discogs_date_changed = getdate() 
WHERE discogs_id = 3110951

SELECT *
FROM RECORDS
ROLLBACK

SELECT *
FROM genres
JOIN records_genres ON genres.genre_id = records_genres.genre_id
JOIN records ON records_genres.discogs_id = records.discogs_id
JOIN libraries ON records.discogs_id = libraries.discogs_id 
WHERE records.discogs_id = 14881 AND username = 'user'

BEGIN TRANSACTION 
insert into libraries (username, discogs_id)
values ('user', 3110951)

SELECT *
FROM libraries
ROLLBACK

select * 
from records
JOIN records_artists ON records.discogs_id = records_artists.discogs_id
JOIN artists ON records_artists.artist_id = artists.artist_id
WHERE artists.name LIKE '%queen%'

INSERT INTO collections (username, name)
VALUES ('aseelt', 'second')

INSERT INTO records_collections (discogs_id, collection_id)
VALUES (18038, 1)

INSERT INTO libraries (username, discogs_id)
VALUES ('jakel', 18038), ('aseelt', 81013)

SELECT username, count(username) AS count
FROM libraries
WHERE username = 'aseelt'
GROUP BY username

SELECT count(username) AS count
FROM libraries

SELECT TOP 2 *
FROM libraries
ORDER BY created_date DESC

SELECT TOP 2 username, created_date
FROM libraries

UPDATE collections
SET name = 'firstupdated'
WHERE username = 'aseelt' AND name = 'first'

SELECT count(username) AS count
FROM collections
WHERE username = 'aseelt'
GROUP BY username

SELECT collections.collection_id, username, name, is_private, collections.was_premium, discogs_id
FROM collections
JOIN records_collections ON collections.collection_id = records_collections.collection_id
WHERE username = 'aseelt' AND name = 'second'

SELECT username, name, count(records_collections.discogs_id) AS count
FROM collections
JOIN records_collections ON collections.collection_id = records_collections.collection_id
WHERE username = 'aseelt' AND name = 'firstupdated'
GROUP BY username, name

SELECT count(artists.artist_id) AS count
FROM artists
JOIN records_artists ON artists.artist_id = records_artists.artist_id
JOIN records ON records_artists.discogs_id = records.discogs_id
JOIN libraries ON records.discogs_id = libraries.discogs_id
WHERE username = 'aseelt'

SELECT artists.name, count(records.discogs_id) AS record_count
FROM artists
JOIN records_artists ON artists.artist_id = records_artists.artist_id
JOIN records ON records_artists.discogs_id = records.discogs_id
JOIN libraries ON records.discogs_id = libraries.discogs_id
--WHERE username = 'aseelt'
GROUP BY artists.name

SELECT country, count (discogs_id) AS record_count
FROM records
GROUP BY country
ORDER by country

SELECT *
FROM artists
JOIN records_artists ON artists.artist_id = records_artists.artist_id
JOIN records ON records_artists.discogs_id = records.discogs_id
JOIN libraries ON records.discogs_id = libraries.discogs_id
WHERE username = 'aseelt'

SELECT *
FROM collections 
JOIN records_collections ON collections.collection_id = records_collections.collection_id
WHERE username = 'aseelt'

BEGIN TRANSACTION
UPDATE rc
SET rc.is_active = 0, rc.updated_date = getdate()
FROM collections AS c
JOIN records_collections AS rc ON c.collection_id = rc.collection_id
WHERE username = 'aseelt'

SELECT *
FROM collections 
JOIN records_collections ON collections.collection_id = records_collections.collection_id
WHERE username = 'aseelt'
ROLLBACK