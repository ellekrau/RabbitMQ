# Topics

Recebimento de mensagens baseadas em um tópico.

Implementação guiada pela documentação do RabbitMQ: https://www.rabbitmq.com/tutorials/tutorial-five-dotnet.html

### Implementação

Criação de consumers "seletivos" de recebimento dos logs com a utilização de uma exchange do tipo Topic.

Producer: Criação de logs com os a identificação da máquina (ZAYA, BETA e ORIO) e grau de severidade (ERROR, WARN e INFO).
Envio desses logs para uma exchange do tipo TOPIC, com o valor de severidade + id da máquina como routing key (Ex.: ZAYA.ERROR).

Consumers: Solicitação via console de quais binding keys a server-named queue deverá ser ligada. Criação de uma server-named queue com bind na topic exchange e routing keys informadas.
Recebimento de logs filtrados de acordo com os binding criados.

### Resultado
![image](https://user-images.githubusercontent.com/52663536/143906583-305b8fb9-c1ca-4769-8fd5-e0ed5360bc59.png)
