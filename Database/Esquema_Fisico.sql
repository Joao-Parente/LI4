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
  `idEmpregado` INT NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(100) NOT NULL,
  `password` VARCHAR(100) NOT NULL,
  `nome` VARCHAR(100) NULL,
  `eGestor` TINYINT NULL,
  PRIMARY KEY (`idEmpregado`))
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
-- Table `LI_Database`.`Reclamacao`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`Reclamacao` (
  `IDPedido` INT NOT NULL,
  `motivo` VARCHAR(100) NULL,
  `assunto` VARCHAR(150) NULL,
  PRIMARY KEY (`IDPedido`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LI_Database`.`Pedido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LI_Database`.`Pedido` (
  `idPedido` INT NOT NULL,
  `idProduto` INT NOT NULL,
  `idEmpregado` INT NOT NULL,
  `idCliente` VARCHAR(100) NOT NULL,
  `avaliacao` INT NULL,
  `quantidade` INT NOT NULL,
  `data_hora` DATETIME NOT NULL,
  INDEX `idCliente_idx` (`idCliente` ASC) VISIBLE,
  INDEX `idProduto_idx` (`idProduto` ASC) VISIBLE,
  INDEX `idEmpregado_idx` (`idEmpregado` ASC) VISIBLE,
  INDEX `idPedido_idx` (`idPedido` ASC) VISIBLE,
  CONSTRAINT `idCliente`
    FOREIGN KEY (`idCliente`)
    REFERENCES `LI_Database`.`Cliente` (`email`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `idProduto`
    FOREIGN KEY (`idProduto`)
    REFERENCES `LI_Database`.`Produto` (`idProduto`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `idEmpregado`
    FOREIGN KEY (`idEmpregado`)
    REFERENCES `LI_Database`.`Empregado` (`idEmpregado`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `idPedido`
    FOREIGN KEY (`idPedido`)
    REFERENCES `LI_Database`.`Reclamacao` (`IDPedido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
