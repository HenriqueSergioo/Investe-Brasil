export class UsuarioAutenticadoEntity {
  codigoUsuario = 0;
  nomeUsuario = '';
  descricaoUsuario = '';
  token = '';

  constructor(init?: Partial<UsuarioAutenticadoEntity>) {
    Object.assign(this, init);
  }
}
