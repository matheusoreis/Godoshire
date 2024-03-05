import 'package:godoshire_servidor/server/server_setup.dart';
import 'package:godoshire_servidor/servidor.dart';

void main() {
  final ServerSetup serverSetup = ServerSetup();

  /// Inicia o servidor;
  ///
  /// Ao iniciar o programa o servidor será iniciado chamando o método `_startServer` atráves
  /// do callable object `serverSetup`.;
  serverSetup();
}
