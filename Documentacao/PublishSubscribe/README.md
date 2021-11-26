## Publish / Subscribe

- O envio de uma mensagem para vários consumers

Implementação guiada pela documentação do RabbitMQ: https://www.rabbitmq.com/tutorials/tutorial-three-dotnet.html

O producer envia a mensagem para uma exchange do tipo fanout, qual "espalha" a mensagem nas queues ligadas a ela (binding).
O consumer declara uma queue served-name e liga ela a uma exchange do tipo fanout (binding), com isso recebe todas as mensagens direcionadas a essa exchange

![image](https://user-images.githubusercontent.com/52663536/143658254-fa7bbb00-6243-4584-9ade-4a9f71026570.png)
