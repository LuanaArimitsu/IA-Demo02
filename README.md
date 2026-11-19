# IA-Demo02

Demo de estudo criada durante o curso **Fundamentos de IA Generativa** do balta.io, para apresentar as bibliotecas de IA do ecossistema .NET.

## Bibliotecas utilizadas

- **Microsoft.Extensions.AI**: camada unificada para trabalhar com modelos de IA no .NET, com recursos como cache de respostas via `ChatClientBuilder`.
- **Microsoft.Extensions.AI.Abstractions**: define as interfaces base, como `IChatClient` e `ChatMessage`, permitindo trocar o provedor de IA sem alterar o código da aplicação.
- **Microsoft.Extensions.AI.OpenAI**: integração com os modelos da OpenAI através do `IChatClient`.
- **OllamaSharp**: cliente para o Ollama, usado para rodar modelos localmente.

## O que a demo mostra

- Uso do **Ollama** (modelo local) em desenvolvimento e da **OpenAI** em produção, com o mesmo código graças ao `IChatClient`.
- Endpoint `POST /`: envia uma pergunta simples ao modelo.
- Endpoint `POST /v2`: usa um *system prompt* para definir o comportamento do modelo.
- **Cache de respostas** com `IDistributedCache`, evitando nova chamada ao modelo para perguntas repetidas.
