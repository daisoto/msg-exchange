CREATE TABLE messages (
    id SERIAL PRIMARY KEY,
    content VARCHAR(128) NOT NULL,
    timestamp TIMESTAMPTZ NOT NULL,
    sequence_number BIGINT NOT NULL
);