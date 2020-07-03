-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema li_database
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema li_database
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `li_database` DEFAULT CHARACTER SET utf8 ;
USE `li_database` ;

-- -----------------------------------------------------
-- Table `li_database`.`cliente`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`cliente` (
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nome` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`email`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`empregado`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`empregado` (
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nome` VARCHAR(100) NULL DEFAULT NULL,
  `eGestor` TINYINT NULL DEFAULT NULL,
  PRIMARY KEY (`email`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`pedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`pedido` (
  `idPedido` INT NOT NULL AUTO_INCREMENT,
  `idCliente` VARCHAR(100) NOT NULL,
  `idEmpregado` VARCHAR(100) NOT NULL,
  `data_hora` DATETIME NOT NULL,
  PRIMARY KEY (`idPedido`),
  INDEX `idCliente_idx` (`idCliente` ASC) VISIBLE,
  INDEX `fk_idEmpregado_idx` (`idEmpregado` ASC) VISIBLE,
  CONSTRAINT `fk_idCliente`
    FOREIGN KEY (`idCliente`)
    REFERENCES `li_database`.`cliente` (`email`),
  CONSTRAINT `fk_idEmpregado`
    FOREIGN KEY (`idEmpregado`)
    REFERENCES `li_database`.`empregado` (`email`))
ENGINE = InnoDB
AUTO_INCREMENT = 84
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
  `imagem` MEDIUMBLOB NULL DEFAULT NULL,
  PRIMARY KEY (`idProduto`))
ENGINE = InnoDB
AUTO_INCREMENT = 20
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`listapedidos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`listapedidos` (
  `idPedido` INT NOT NULL,
  `idProduto` INT NOT NULL,
  `quantidade` INT NOT NULL DEFAULT '0',
  PRIMARY KEY (`idPedido`, `idProduto`),
  INDEX `fk_idProduto_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_idPedido`
    FOREIGN KEY (`idPedido`)
    REFERENCES `li_database`.`pedido` (`idPedido`),
  CONSTRAINT `fk_idProduto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `li_database`.`produto` (`idProduto`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`produtosfavoritos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`produtosfavoritos` (
  `idCliente` VARCHAR(100) NOT NULL,
  `idProduto` INT NOT NULL,
  PRIMARY KEY (`idCliente`, `idProduto`),
  INDEX `fk_table1_Produto1_idx` (`idProduto` ASC) VISIBLE,
  CONSTRAINT `fk_table1_Cliente1`
    FOREIGN KEY (`idCliente`)
    REFERENCES `li_database`.`cliente` (`email`),
  CONSTRAINT `fk_table1_Produto1`
    FOREIGN KEY (`idProduto`)
    REFERENCES `li_database`.`produto` (`idProduto`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `li_database`.`reclamacao`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `li_database`.`reclamacao` (
  `idPedido` INT NOT NULL,
  `motivo` VARCHAR(100) NOT NULL,
  `assunto` VARCHAR(150) NOT NULL,
  `data_hora` DATETIME NULL DEFAULT NULL,
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
