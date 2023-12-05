SELECT * 
FROM records
JOIN records_artists ON records.discogs_id = records_artists.discogs_id
JOIN artists ON records_artists.artist_id = artists.artist_id

JOIN records_extra_artists ON records.discogs_id = records_extra_artists.discogs_id
JOIN artists AS extra_artists ON records_extra_artists.extra_artist_id = extra_artists.artist_id

JOIN images ON records.discogs_id = images.discogs_id

JOIN records_labels ON records.discogs_id = records_labels.discogs_id
JOIN labels ON records_labels.label_id = labels.label_id

JOIN tracks ON records.discogs_id = tracks.discogs_id

JOIN barcodes ON records.discogs_id = barcodes.discogs_id

JOIN records_formats ON records.discogs_id = records_formats.discogs_id
JOIN formats ON records_formats.format_id = formats.format_id 

JOIN records_genres ON records.discogs_id = records_genres.discogs_id
JOIN genres ON records_genres.genre_id = genres.genre_id