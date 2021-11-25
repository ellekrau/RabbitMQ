# Work Queues
- Implementação de um load balancer
- Usado para distribuir tasks custosas em múltiplos workers (executores)
- Maneira fácil de escalabilidade, pois num aumento de carga é possível adicionar mais instâncias de uma aplicação lendo uma mesma fila
- Isolar a execução de tasks custosas que não precisam ser executadas de forma síncrona (imediatamente), onde a aplicação aguarda o fim da execução para passar para a etapa seguinte
- A task é encapsulada em uma mensagem e enviada para uma fila
- Um (ou mais) processo worker (executor) executando em background consome essa fila e eventualmente executa a task
- Quando vários workers são executados ao mesmo tempo, as tasks são compartilhadas entre eles
- O conceito é muito utilizado em aplicações web onde é impossível lidar com uma task complexa durante a execução de uma HTTP request

## Implementação ##
A aplicação NewTask envia uma mensagem para uma fila e as aplicações Worker consomem mensagens dessa fila e aplicam um Thread.Sleep de 1000 milliseconds para cada ponto no contúdo da mensagem.
Importante: O comando Console.ReadLine está segurando a Thread e faz com que o console não feche, proporcionando a visualização dos logs de execução dos Workers.

## Pré-requisito para execução  ##
- Instalação local do RabbitMQ

## Execução ##
- Clonar o projeto na máquina

### Execução da aplicação NewTask - Publisher ###
1. Abrir o powershel
2. cd {pastaDoProjeto}/NewTask
3. dotnet build
4. dotnet run

### Execução da aplicação Worker - Consumer 1 ###
1. Abrir o powershel
2. cd {pastaDoProjeto}/NewTask
3. dotnet build
4. dotnet run

### Execução da aplicação Worker - Consumer 2 ###
1. Abrir o powershel
2. cd {pastaDoProjeto}/NewTask
3. dotnet build
4. dotnet run

![image](https://user-images.githubusercontent.com/52663536/143356247-ce4c15e8-4556-4ea6-89f2-357ead236065.png)

