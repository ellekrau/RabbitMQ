## Routing
Recebimento seletivo de mensagens.

Implementação guiada pela documentação do RabbitMQ: https://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html

### Implementação
Criação de um sistema de logs com mensageria.

Producer: Criação de logs com os grau de severidade [E]rror, [W]arn e [I]nfo e envio para uma exchange do tipo DIRECT com a severidade no routing-key.

Consumers: Criação de uma server-named queue com bind(s) em uma exchange do tipo DIRECT e routing-key de acordo com a severidade proposta para a aplicação, e apresentação em tela.
1. Recebimento de logs com maior importância (ERROR)
2. Recebimento de logs de média+ importância (ERROR, WARN)
3. Recebimento de todos os logs (ERROR, WARN e INFO)

### Resultado
![image](https://user-images.githubusercontent.com/52663536/143725884-1018e96b-ad5d-4790-8d5a-9f85aeabb317.png)

![image](https://user-images.githubusercontent.com/52663536/143725894-e567ed81-260f-4669-aacf-70ca7df444b7.png)

![image](https://user-images.githubusercontent.com/52663536/143725908-dc2b68a5-d2ab-4635-8b3a-7d00d244b030.png)

![image](https://user-images.githubusercontent.com/52663536/143725410-0cdf34b1-223d-4a30-9d8e-d37ec285e598.png)
