## Projeto Cantina

## Premissa sistêmica:
- Permitir o cadastro de um responsável por um ou mais estudantes.
- Permitir visualizar relatório de consumo/gastos financeiro na cantina. Os itens consumidos deverão ser apontados manualmente no sistema por um usuário sistêmico devidamente cadastrado, identificando o estudante que consumiu o item/valor/quantidade/horario.
- Permitir o cadastro de produtos/descrição/quantidade que estarão à venda na cantina.
- Permitir o lançamento de consumo do estudante de qualquer item existente e devidamente cadastrado no sistema. Subtrair o item consumido afim de ter um controle básico dos itens consumidos.
- Permitir adicionar crédito ao estudante que será abatido ao consumir os itens na cantina. O valor deste crédito será lançado diretamente pelo usuário do sistema, ficando à sua responsabilidade tal ação.
- Permitir caso o estudante não tenha saldo, consumir itens até alcançar determinado valor (configuravel)

## Tecnologias em uso
- Template responsive Metronic.
- Arquitetura MVC - Model, View, Controller.
- Entity framework 6 - Migrations com MySql Server, validações em backend utilizando Entity validations errors.
- AngularJs - Form validations, directivas, factories, services, filters e injeção de dependências.
- Unit tests - Testes unitário.
