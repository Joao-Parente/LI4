-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema LI_Database
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema LI_Database
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `LI_Database` DEFAULT CHARACTER SET utf8 ;
-- -----------------------------------------------------
-- Schema li_database
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema li_database
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `li_database` DEFAULT CHARACTER SET utf8 ;
USE `LI_Database` ;

-- -----------------------------------------------------
-- Table `LI_Database`.`Cliente`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`Cliente` (
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nome` VARCHAR(100) NULL,
  PRIMARY KEY (`email`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LI_Database`.`Empregado`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`Empregado` (
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nome` VARCHAR(100) NULL,
  `eGestor` TINYINT NULL,
  PRIMARY KEY (`email`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LI_Database`.`Produto`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`Produto` (
  `idProduto` INT NOT NULL AUTO_INCREMENT,
  `tipo` VARCHAR(50) NOT NULL,
  `nome` VARCHAR(100) NOT NULL,
  `detalhes` VARCHAR(150) NULL,
  `disponibilidade` TINYINT NOT NULL,
  `preco` DECIMAL(5,2) NOT NULL,
  `imagem` TINYBLOB NULL,
  PRIMARY KEY (`idProduto`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LI_Database`.`Pedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`Pedido` (
  `idPedido` INT NOT NULL AUTO_INCREMENT,
  `idCliente` VARCHAR(100) NOT NULL,
  `idEmpregado` VARCHAR(100) NOT NULL,
  `data_hora` DATETIME NOT NULL,
  PRIMARY KEY (`idPedido`),
  INDEX `idCliente_idx` (`idCliente` ASC) VISIBLE,
  INDEX `fk_idEmpregado_idx` (`idEmpregado` ASC) VISIBLE,
  CONSTRAINT `fk_idCliente`
    FOREIGN KEY (`idCliente`)
    REFERENCES `LI_Database`.`Cliente` (`email`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_idEmpregado`
    FOREIGN KEY (`idEmpregado`)
    REFERENCES `LI_Database`.`Empregado` (`email`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LI_Database`.`Reclamacao`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`Reclamacao` (
  `idPedido` INT NOT NULL,
  `motivo` VARCHAR(100) NOT NULL,
  `assunto` VARCHAR(150) NOT NULL,
  `data_hora` DATETIME NULL,
  INDEX `idPedido_idx` (`idPedido` ASC) VISIBLE,
  PRIMARY KEY (`idPedido`),
  CONSTRAINT `idPedido`
    FOREIGN KEY (`idPedido`)
    REFERENCES `LI_Database`.`Pedido` (`idPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LI_Database`.`ListaPedidos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`ListaPedidos` (
  `idPedido` INT NOT NULL,
  `idProduto` INT NOT NULL,
  `quantidade` INT NOT NULL DEFAULT 0,
  PRIMARY KEY (`idPedido`, `idProduto`),
  INDEX `fk_idProduto_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_idProduto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `LI_Database`.`Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_idPedido`
    FOREIGN KEY (`idPedido`)
    REFERENCES `LI_Database`.`Pedido` (`idPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LI_Database`.`ProdutosFavoritos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`ProdutosFavoritos` (
  `idCliente` VARCHAR(100) NOT NULL,
  `idProduto` INT NOT NULL,
  PRIMARY KEY (`idCliente`, `idProduto`),
  INDEX `fk_table1_Produto1_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_table1_Cliente1`
    FOREIGN KEY (`idCliente`)
    REFERENCES `LI_Database`.`Cliente` (`email`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_table1_Produto1`
    FOREIGN KEY (`idProduto`)
    REFERENCES `LI_Database`.`Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `li_database` ;

-- -----------------------------------------------------
-- Table `li_database`.`cliente`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`cliente` (
  `idCliente` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nome` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`idCliente`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`empregado`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`empregado` (
  `idEmpregado` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nome` VARCHAR(100) NULL DEFAULT NULL,
  `eGestor` TINYINT NULL DEFAULT NULL,
  PRIMARY KEY (`idEmpregado`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`pedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`pedido` (
  `idPedido` INT NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`idPedido`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`produto`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`produto` (
  `idProduto` INT NOT NULL AUTO_INCREMENT,
  `tipo` VARCHAR(50) NOT NULL,
  `nome` VARCHAR(100) NOT NULL,
  `detalhes` VARCHAR(150) NULL DEFAULT NULL,
  `disponibilidade` TINYINT NOT NULL,
  `preco` DECIMAL(5,2) NOT NULL,
  `imagem` TINYBLOB NULL DEFAULT NULL,
  PRIMARY KEY (`idProduto`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`reclamacao`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`reclamacao` (
  `idPedido` INT NOT NULL,
  `motivo` VARCHAR(100) NOT NULL,
  `assunto` VARCHAR(150) NOT NULL,
  PRIMARY KEY (`idPedido`),
  INDEX `idPedido_idx` (`idPedido` ASC) VISIBLE,
  CONSTRAINT `idPedido`
    FOREIGN KEY (`idPedido`)
    REFERENCES `li_database`.`pedido` (`idPedido`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
