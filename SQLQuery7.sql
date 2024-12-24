CREATE TABLE AllStations (
    id INT IDENTITY(1,1) PRIMARY KEY,
    station_code VARCHAR(10) unique  NOT NULL,
    station_name VARCHAR(255) NOT NULL,
    state NVARCHAR(255),
    zone NVARCHAR(10),
    address varchar(200),
    latitude FLOAT,
    longitude FLOAT
);
