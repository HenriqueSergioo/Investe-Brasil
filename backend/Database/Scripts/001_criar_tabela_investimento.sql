CREATE TABLE IF NOT EXISTS investimento (
    id BIGSERIAL PRIMARY KEY,
    proprietario VARCHAR(200) NOT NULL,
    data_criacao DATE NOT NULL,
    valor_inicial NUMERIC(18,2) NOT NULL CHECK (valor_inicial > 0),
    cpf VARCHAR(11) NULL,
    tipo_investimento VARCHAR(80) NULL,
    instituicao VARCHAR(120) NULL,
    liquidez VARCHAR(40) NULL,
    data_vencimento DATE NULL,
    objetivo VARCHAR(80) NULL,
    observacao VARCHAR(500) NULL,
    data_resgate DATE NULL,
    situacao VARCHAR(20) NOT NULL
);

ALTER TABLE investimento ADD COLUMN IF NOT EXISTS cpf VARCHAR(11);
ALTER TABLE investimento ADD COLUMN IF NOT EXISTS tipo_investimento VARCHAR(80);
ALTER TABLE investimento ADD COLUMN IF NOT EXISTS instituicao VARCHAR(120);
ALTER TABLE investimento ADD COLUMN IF NOT EXISTS liquidez VARCHAR(40);
ALTER TABLE investimento ADD COLUMN IF NOT EXISTS data_vencimento DATE;
ALTER TABLE investimento ADD COLUMN IF NOT EXISTS objetivo VARCHAR(80);
ALTER TABLE investimento ADD COLUMN IF NOT EXISTS observacao VARCHAR(500);

CREATE INDEX IF NOT EXISTS ix_investimento_proprietario ON investimento (proprietario);
CREATE INDEX IF NOT EXISTS ix_investimento_data_criacao ON investimento (data_criacao);
