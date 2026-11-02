export class UsuarioAutenticadoEntity {
  id = 0;
  nome = '';
  usuario = '';
  token = '';

  constructor(init?: Partial<UsuarioAutenticadoEntity>) {
    Object.assign(this, init);
  }
}
