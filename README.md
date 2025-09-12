# ImporterBIM

> Ferramenta para Unity3D com propósito de oferecer suporte a importação, navegação e visualização de modelos e metadados BIM.

![Unity](https://img.shields.io/badge/Unity-2022.3+-blue.svg)
![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow.svg)

## 📋 Sobre o Projeto

**ImporterBIM** é uma solução desenvolvida para Unity3D que visa facilitar a importação, visualização e navegação de modelos BIM (Building Information Modeling) em aplicações de realidade virtual, aumentada e gêmeos digitais. O projeto foca especialmente em aplicações para usinas hidrelétricas e infraestruturas complexas.

### 🎯 Objetivos
- Importação de modelos 3D (.glb/.gltf) em runtime
- Sistema de navegação 3D intuitivo para modelos BIM
- Futura integração com metadados IFC
- Interface amigável para seleção e carregamento de arquivos
- Performance otimizada para modelos de alta complexidade

## ✨ Features Atuais

- ✅ **Sistema de Navegação 3D Completo**
  - Rotação orbital com mouse
  - Movimentação livre (WASD + mouse)
  - Zoom suave (scroll)
  - Centralização automática em objetos
  - Reset de visualização

- ✅ **Interface de Seleção de Arquivos**
  - Integração com Simple File Browser
  - Filtros por extensão
  - Validação de arquivos
  - Tratamento de erros

- ✅ **Suporte a Modelos Nativos Unity**
  - Carregamento de FBX, OBJ
  - Instanciação automática na cena
  - Ajuste de câmera baseado no bounding box

## 🚀 Features Planejadas

- 🔄 **Integração glTFast** (Próxima versão)
  - Carregamento assíncrono de .glb/.gltf
  - Suporte a materiais PBR
  - Indicadores de progresso

- 📊 **Metadados BIM** (Futuro)
  - Integração com IFCOpenShell
  - Visualização de propriedades de objetos
  - Pipeline IFC→glTF

- 🎨 **Interface Avançada** (Futuro)
  - UI/UX otimizada
  - Sistema de layers
  - Ferramentas de medição

## 🛠️ Requisitos Técnicos

### Unity
- **Versão:** Unity 2022.3 LTS ou superior
- **Plataformas:** Windows, macOS, Linux
- **Render Pipeline:** URP/HDRP recomendado

### Dependências

| Package/Plugin | Versão | Status | Descrição |
|---|---|---|---|
| **Unity glTFast** | `6.0+` | 🔄 Em integração | Carregamento de modelos glTF/GLB em runtime |
| **Simple File Browser** | `Latest` | ✅ Integrado | Interface de seleção de arquivos |
| **IFCOpenShell** | `TBD` | 📋 Planejado | Processamento de arquivos IFC |

## 📦 Instalação

### 1. Clone o Repositório
```bash
git clone https://github.com/Luisguiv/ImporterBIM.git
cd ImporterBIM
```

### 2. Configuração do Unity
1. Abra o Unity Hub
2. Adicione o projeto clonado
3. Abra com Unity 2022.3 LTS ou superior

### 3. Instalar Dependências

#### Unity glTFast
```
1. Window → Package Manager
2. Add package by name: com.unity.cloud.gltfast
3. Install
```

#### Simple File Browser
```
1. Download: https://github.com/yasirkula/UnitySimpleFileBrowser/releases
2. Import para o projeto Unity
3. Ou via Package Manager (Git URL)
```

### 4. Configuração da Cena
1. Abra a cena principal em `Assets/Scenes/Main.unity`
2. Configure os GameObjects necessários
3. Teste a funcionalidade básica

## 🎮 Como Usar

### Navegação 3D
- **Rotação:** Mouse + Clique direito
- **Movimentação:** WASD
- **Zoom:** Scroll do mouse
- **Reset:** Tecla R
- **Centralizar:** Tecla F

### Carregar Modelos
1. Clique no botão "Load Model"
2. Selecione um arquivo (.fbx, .obj, futuramente .glb/.gltf)
3. O modelo será carregado automaticamente na cena
4. Use os controles de navegação para explorar

## 🏗️ Estrutura do Projeto

```
ImporterBIM/
├── Assets/
│   ├── Scripts/
│   │   ├── Navigation/          # Controles de navegação 3D
│   │   ├── FileHandling/        # Carregamento de arquivos
│   │   ├── UI/                  # Interface do usuário
│   │   └── Utils/               # Utilitários gerais
│   ├── Scenes/
│   │   └── Main.unity           # Cena principal
│   ├── Models/                  # Modelos de teste
│   └── UI/                      # Assets de interface
├── Packages/
│   └── manifest.json            # Dependências do projeto
├── Documentation/
│   ├── API.md                   # Documentação da API
│   └── Examples.md              # Exemplos de uso
└── README.md
```

## 🗺️ Roadmap

### Versão 1.0
- [x] Sistema de navegação 3D
- [x] Interface de seleção de arquivos
- [x] Carregamento de modelos Unity nativos
- [x] Documentação básica

### Versão 2.0
- [ ] Integração completa com glTFast
- [ ] Carregamento assíncrono
- [ ] Suporte a .glb/.gltf
- [ ] Melhorias de performance

### Versão 3.0
- [ ] Integração com IFCOpenShell
- [ ] Visualização de metadados BIM
- [ ] Pipeline IFC→glTF
- [ ] Interface avançada

### Futuras Versões
- [ ] Multi-plataforma (WebGL, Mobile)
- [ ] Colaboração multi-usuário
- [ ] Integração com sensores IoT
- [ ] Análises visuais avançadas

## 🔗 Links Úteis

- [Unity glTFast Documentation](https://docs.unity3d.com/Packages/com.unity.cloud.gltfast@latest/)
- [Simple File Browser](https://github.com/yasirkula/UnitySimpleFileBrowser)
- [IFCOpenShell](https://ifcopenshell.org/)
- [Documentação BIM](https://www.buildingsmart.org/)

## 🔗 Repositório de Modelos

- [Common 3D Test Models](https://github.com/alecjacobson/common-3d-test-models)
- [glTF Sample Models](https://github.com/KhronosGroup/glTF-Sample-Models)
- [IfcSampleFiles](https://github.com/youshengCode/IfcSampleFiles)
