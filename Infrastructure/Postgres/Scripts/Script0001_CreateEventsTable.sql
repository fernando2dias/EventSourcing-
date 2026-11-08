CREATE TABLE IF NOT EXISTS events (
	id UUID PRIMARY KEY,
	stream_id VARCHAR(100) NOT NULL,
	event_type VARCHAR(250) NOT NULL,
	data_json JSONB NOT NULL,
	version INT NOT NULL,
	occurred_on TIMESTAMPTZ NOT NULL,
	CONSTRAINT unique_stream_version UNIQUE (stream_id, version)
);

CREATE INDEX IF NOT EXISTS idx_events_stream_id ON events (stream_id, version ASC);
