# bookingbuddy
Projeto ESA 2023/2024

# Git Guide
O branch _main_ vai conter as implementações a cada Sprint. Só deve ser atualizado quando fecharmos um sprint e todos os testes passarem.
O branch _dev_ vai conter as implementações atualizadas até a data.

- Sempre que abrem o projeto é necessário fazer: `git pull` para obter o conteúdo atualizado do repositório;
- Nova funcionalidade = novo branch com base no branch _dev_: `git checkout -b nova_funcionalidade dev` para criar um branch e mudar logo para o mesmo com base num branch de referência (neste caso _dev_)
- Acabada a implementação: 
  - `git status` para ver os ficheiros que foram modificados;
  - `git add nome_do_ficheiro_que_quiserem_adicionar` (ou `git add .` com especial cuidado para não adicionarem coisas que não devem) para adicionarem os ficheiros alterados;
  - `git commit -m "Mensagem explicativa como referido a cima"` para adicionar mensagem ao commit;
  - `git push` para enviar o commit para o branch.

- Agora é necessário colocar as alterações no branch _dev_: 
  - Criar um novo pull-request referindo o branch que contém a nova funcionalidade e aguardar a aprovação do mesmo.

- Agora o branch _dev_ já contém todas as alterações!

> Para passar de um branch para o anterior basta fazer `git checkout -` (git checkout seguido de um traço).
